using AutoMapper;
using Entities.Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using LoggerService;

namespace ApiServer.Controllers
{
    // Synchronous method
    // ControllerBase is controller without view support - (view we do not need-it)

    [Route("api/[controller]")] // general rule
    [ApiController]
    public class AlimentController : ControllerBase
    {
        // bad way to do it - will instantiate concreate method of this class to have data
        //private readonly MockCommanderRepo _repository = new MockCommanderRepo();

        //Good create constructor for dependency to be injected
        private readonly IAlimentRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _log;           // logging with native Microsoft.Extensions.Logging
        private readonly ILoggerManager _logm; // logging with NLog

        //public ILogger Log => _log; // diferent way to use Log instead of _log (Microsoft.Extensions.Logging)

        public AlimentController(IAlimentRepository repository, IMapper mapper, ILogger<AlimentController> log, ILoggerManager logm)
        {
            _repository = repository;
            _mapper = mapper;
            _log = log;
            _logm = logm;
        }
        // GET api/aliment
        [HttpGet]
        public ActionResult<IEnumerable<AlimentReadDto>> GetAllAliments()
        {
            // return list of models mapped by AutoMapper
            var items = _repository.GetAllAliments();

            _log.LogWarning("LogInformation from controller");
            _logm.LogWarn("LogInformation from controller NLog");

            // give 200 (Ok) response
            return Ok(_mapper.Map<IEnumerable<AlimentReadDto>>(items));
        }

        // GET api/aliment/{id}
        [HttpGet("{id}", Name = "GetAlimentById"), ActionName("getactionname")]  
                //{id} parameter is required,  use {id?} if optional, 
                // "Name" property with value "GetAlimentById" is used for CreatedAtRoute function to send the 201Created response to POST request
                // "ActionName" attribute used as route
        public ActionResult<AlimentReadDto> GetAlimentById(int id)
        {
            var item = _repository.GetAlimentById(id);

            _log.LogInformation($"LogInformation from controller id={id}");
            _logm.LogInfo($"LogInformation from controller id={id} NLog");

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
        public ActionResult<AlimentReadDto> CreateAliment(AlimentCreateDto alimentCreateDto)
        {
            var alimentModel = _mapper.Map<Aliment>(alimentCreateDto);
            _repository.CreateAliment(alimentModel);
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
        public ActionResult UpdateAliment(int id, AlimentUpdateDto commandUpdateDto)
        {
            var alimentModelFromRepo = _repository.GetAlimentById(id);
            if (alimentModelFromRepo == null)
                return NotFound();

            _mapper.Map(commandUpdateDto, alimentModelFromRepo);
            _repository.UpdateAliment(alimentModelFromRepo);
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
        public ActionResult PartialUpdate(int id, JsonPatchDocument<AlimentUpdateDto> patchDoc)
        {
            var alimentModelFromRepo = _repository.GetAlimentById(id);
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
            _repository.UpdateAliment(alimentModelFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }

        //DELETE api/aliment/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteAliment(int id)
        {
            var alimentModelFromRepo = _repository.GetAlimentById(id);
            if (alimentModelFromRepo == null)
                return NotFound();

            _repository.DeleteAliment(alimentModelFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }


    }
}
