using ApiServer.Logic;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;

namespace ApiServer.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SingleThreadLockController : ControllerBase
    {
        // produce delay on some connections
        public static readonly object LockObject1 = new object();


        [HttpPut("{id}") ]
        //[Route("api/Public/SendCommissioning/{serial}/{withChildren}")]
        public ActionResult PutById(int id)
        {
            lock (LockObject1)
            {
                //Do stuff
                // this will aquire a lock on a single thread but
                // web requests will go on hanging for long time (Which may result in time outs from the client end).
            }


            // creates a waiting list, used for MemoryCaching
            try
            {
                // if there is another thread that is executing will wait for that to finish and then start
                MemoryCache<LockedObject2>.StartTransaction("key1");
                // Object is locked, do operations here
}
            finally
            {
                MemoryCache<LockedObject2>.EndTransaction("key1");
                // Object is released, next in line will start
            }
            return Ok();
        }


    }
    public class LockedObject2 { public int Id { get; set; } public string Name { get; set; } }
}
