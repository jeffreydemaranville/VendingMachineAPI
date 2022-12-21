using Microsoft.AspNetCore.Mvc;
using VendingMachineAPI.Models;

namespace VendingMachineAPI.Controllers
{
    [ApiController]
    [Route("~/")]
    public class ItemController : Controller
    {

        //Logger and MachineState instantiated with Controller instance for persistence of state
        private readonly ILogger<ItemController> _logger;

        /// <summary>
        /// Singleton that is instantiated with the Controller.  Used to persist the state of the vending machine.
        /// </summary>
        private readonly MachineState _machineState;


        public ItemController(ILogger<ItemController> logger, MachineState machineState)
        {
            _logger = logger;
            _machineState = machineState;
        }


        /// <summary>
        /// Adds one to the number of coins tracked by MachineState.  Responds with the No Content (204) status code and number of coins (X-Coins).
        /// </summary>
        [HttpPut]
        [Route("~/")]
        public void InsertCoin()
        {
            _machineState.Coins++;
            Response.ContentType = "application/json";
            Response.StatusCode = 204;
            Response.Headers.Add("X-Coins", _machineState.Coins.ToString());
        }

        /// <summary>
        /// Removes all coins tracked by MachineState and resets number of items vended.  Responds with the No Content (204) status code and number of coins (X-Coins).
        /// </summary>
        [HttpDelete]
        public void RemoveCoins()
        {
            //End transaction => reset Coins, Respond            
            Response.Headers.Add("X-Coins", _machineState.Coins.ToString());
            Response.ContentType = "application/json";
            Response.StatusCode = 204;
            _machineState.Coins = 0;            
        }

        /// <summary>
        /// Gets the quantities of each Item in the inventory.  Responds with the OK (200) status code.
        /// </summary>
        /// <returns>An integer array of quantities.</returns>
        [HttpGet]
        [Route("~/inventory")]
        public JsonResult GetInventory()
        {
            Response.ContentType = "application/json";
            Response.StatusCode = 200;
            return Json(new { itemQuantities = _machineState.Items.Select(x => x.Quantity) });
        }

        /// <summary>
        /// Gets the quantity of a single Item in the inventory.  Responds with OK (200) status code if the given id is valid.
        /// </summary>
        /// <param name="id">The Id value of an Item.</param>
        /// <returns>An integer value representing the number of an item left in inventory.</returns>
        [HttpGet("{id}")]
        [Route("~/inventory/{id}")]
        public JsonResult GetItemQuantity(int id)
        {
            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            return Json(new { itemQuantity = _machineState.Items.Where(x => x.Id == id).Select(x => x.Quantity).SingleOrDefault() });            
        }

        /// <summary>
        /// Processes the transaction of a single item from the vending machine.
        /// </summary>
        /// <param name="id">The id value of an Item to be vended.</param>
        /// <returns>Quantity of items purchased.</returns>
        [HttpPut]
        [Route("~/inventory/{id}")]
        public JsonResult PurchaseItem(int id)
        {
            //Ensure funds have been inserted and item is in stock
            if (_machineState.Coins >= 2 && _machineState.Items.Where(x => x.Id == id).Select(x => x.Quantity).SingleOrDefault() > 0)
            {
                _machineState.Items[id - 1].Quantity--;

                //Respond with OK, # of coins to be returned, # of items vended
                Response.ContentType = "application/json";
                Response.StatusCode = 200;
                Response.Headers.Add("X-Coins", (_machineState.Coins - 2).ToString());
                Response.Headers.Add("X-Inventory-Remaining", _machineState.Items[id - 1].Quantity.ToString());
                _machineState.Coins = 0;                
                return Json(new { quanity = 1 });
            }
            //Item is out of stock => respond with 404, header shows X-Coins available
            else if (_machineState.Items.Where(x => x.Id == id).Select(x => x.Quantity == 0).SingleOrDefault())
            {
                Response.ContentType = "application/json";
                Response.StatusCode = 404;
                Response.Headers.Add("X-Coins", _machineState.Coins.ToString());
                return Json(new { });
            }
            //Not enough $$ => respond 403 && x-coins
            else
            {
                Response.ContentType = "application/json";
                Response.StatusCode = 403;
                Response.Headers.Add("X-Coins", _machineState.Coins.ToString());
                return Json(new { });
            }

        }
    }
}
