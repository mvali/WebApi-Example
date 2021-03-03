using AutoMapper;
using Entities.Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using LoggerService;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Repository.DbData;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    // Synchronous method
    // ControllerBase is controller without view support - (view we do not need-it)

    [Route("api/[controller]")] // general rule
    [ApiController]
    public class AlimentAsyncController : ControllerBase
    {
        // bad way to do it - will instantiate concreate method of this class to have data
        //private readonly MockCommanderRepo _repository = new MockCommanderRepo();

        //Good create constructor for dependency to be injected
        private readonly IAlimentRepositoryAsync _repository;
        private readonly IMapper _mapper;
        private readonly LoggerService.ILoggerManager _logger; // to test logs

        //public AlimentController(IAlimentRepository repository, IMapper mapper, IOptions<ConnectionStrings> connectionStrings2, IConfiguration configuration3)
        //public AlimentAsyncController(IAlimentRepositoryAsync repository, IMapper mapper, ILoggerManager logger)
        public AlimentAsyncController(IAlimentRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            //_logger = logger;
        }
        // GET api/alimentasync
        [HttpGet]
        public async Task <ActionResult<IEnumerable<AlimentReadDto>>> GetAllAliments()
        {
            // return list of models mapped by AutoMapper
            var items = await _repository.GetAllAlimentsAsync();

            var retObj = _mapper.Map<IEnumerable<AlimentReadDto>>(items);


            // give 200 (Ok) response
            return Ok(retObj);
        }

        // GET api/alimentasync/{id}
        //[HttpGet("{id}", Name = "GetAlimentById")]
        [HttpGet("{id}")]
        public async Task<ActionResult<AlimentReadDto>> GetAlimentById(int id)
        {
            var item = await _repository.GetAlimentByIdAsync(id);
            if (item != null)
            {
                // return map of databaseModel done by AutoMapper
                return Ok(_mapper.Map<AlimentReadDto>(item));
            }
            return NotFound();
        }

        // Use Postman-> Body/raw - json
        // POST api/aliment   for Inserting data
        [HttpPost]
        public async Task<ActionResult<AlimentReadDto>> CreateAliment(AlimentCreateDto alimentCreateDto)
        {
            var alimentModel = _mapper.Map<Aliment>(alimentCreateDto);
            await _repository.CreateAlimentAsync(alimentModel); // no need for await here as this is a Method is async as well
            _repository.SaveChanges();

            var alimentReadDto = _mapper.Map<AlimentReadDto>(alimentModel);

            // give 201 (created) result with: location where it was created
            // according REST principle you must pass back the object and location where it was created
            return CreatedAtRoute(nameof(GetAlimentById), new { Id = alimentReadDto.Id }, alimentReadDto);

            // if form is not validated, wil return 400 (Bad Request) message with detailed message
        }

        // PUT request - Full Update: need to supply the entire object even if only one property has changded
        //            - Inefficient (specially for large objects)
        // PATCH is used for partial update 204=code for Updated, client can call object to see if it was updated

        //PUT api/commands/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAliment(int id, AlimentUpdateDto commandUpdateDto)
        {
            var alimentModelFromRepo = await _repository.GetAlimentByIdAsync(id);
            if (alimentModelFromRepo == null)
                return NotFound();

            _mapper.Map(commandUpdateDto, alimentModelFromRepo);
            await _repository.UpdateAlimentAsync(alimentModelFromRepo);
            _repository.SaveChanges();

            return NoContent(); // 204 - No Content
        }


        // PATCH         partial update. Accept an array of operations: add, remove, replace, copy, move, test
        //  - "op" - operation tag indentifier [{"op": "replace", "path": "/howto", "value" : "some new falue"}],
        //                                      {"op": "test",    "path": "/line",  "value":  "test value"}]
        //  - "path" - property to be changed
        // All operations need to be completed successfuly
        // nuget: Microsoft.AspNetCore.JsonPatch
        //                            .Mvc.NewtonsoftJson - core V5

        // PATCH api/commands/{id}
        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialUpdate(int id, JsonPatchDocument<AlimentUpdateDto> patchDoc)
        {
            var alimentModelFromRepo = await _repository.GetAlimentByIdAsync(id);
            if (alimentModelFromRepo == null)
                return NotFound();

            // create a new AlimentUpdateDto from alimentModelFromRepo
            var alimentToPatch = _mapper.Map<AlimentUpdateDto>(alimentModelFromRepo);
            patchDoc.ApplyTo(alimentToPatch, ModelState);// Modelstate makes sure the validation is set (.core v5)
            
            // just rerun the validation with TryValidate
            if (!TryValidateModel(alimentToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(alimentToPatch, alimentModelFromRepo);
            await _repository.UpdateAlimentAsync(alimentModelFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }

        //DELETE api/aliment/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAliment(int id)
        {
            var alimentModelFromRepo = await _repository.GetAlimentByIdAsync(id);
            if (alimentModelFromRepo == null)
                return NotFound();

            await _repository.DeleteAlimentAsync(alimentModelFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }

        [HttpGet]
        [Route("api/alimentasync/cancelationtest")]
        public async Task<ActionResult<IEnumerable<AlimentReadDto>>> CancelationTest(CancellationToken cancellationToken)
        {
            _logger.LogInfo("Starting SlowTask will last for 10sec");

            // slow async action, e.g. call external api
            await Task.Delay(10_000, cancellationToken);

            // return list of models mapped by AutoMapper
            var items = await _repository.GetAllAlimentsAsync();

            var retObj = _mapper.Map<IEnumerable<AlimentReadDto>>(items);

            _logger.LogInfo("Finished 5 seconds delay on SlowTask");

            for (int i = 0; i < 5; i++)// have another 10x1sec delay
            {
                cancellationToken.ThrowIfCancellationRequested();
                // slow non-cancellable work
                Thread.Sleep(1000); _logger.LogInfo($"Passed {i}sec");
            }

            // give 200 (Ok) response
            return Ok(retObj);
        }




    }
}
