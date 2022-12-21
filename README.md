# Vending Machine API
## Overview
 This API was created to simulate vending machine transactions via HTTP, accounting for inventory items in stock, exchange of currency, and distribution of inventory.  In order to accomodate for state and track the inventory, I have utilized a singleton instance to track the vending machine's state.  This allows me to keep track of transactions and inventory between API requests.
 
## Get Started
There are multiple ways to run this application - with OSX-specific files, Windows-specific files, or access the API via the web.

### OSX
> Note: *I have not shipped any software for OSX before and I did not have a machine to test this on, so if for some reason this does not work, refer to instructions for accessing the API via the web.*
1. Open /Release/osx
2. Run the VendingMachineAPI file
3. Access the API at https://localhost:5001 or http://localhost:5000

### Windows
1. Open /Release/windows
2. Run the VendingMachineAPI.exe file
3. Access the API at https://localhost:5001 or http://localhost:5000

### Web
> Note: *Unfortunately, the hosting provider requires a substantial monthly subscription for HTTPS support, so for now this API is only available via HTTP.*
1. Access the API at http://api.jeffdemaranville.com

## Requests
### GET
* **/inventory**
   * returns an array of integers representing the number of each beverage left in the vending machine.  Responds with 200 status code.
* **/inventory/[id]** 
   * [id] being a valid id for the vending machine items.  These values are currently hard-coded in the machine state, and the only viable values present are 1, 2, and 3.  If an appropriate id is provided this will return an integer displaying how many instances of that item are in stock, and also respond with 200 status code.

### PUT
* **/** 
    * Inserts a coin into the machine, and responds with X-Coins (total number of coins inserted) and status code 204.
* **/inventory/[id]** 
    * Processes the transaction for a single item from the vending machine, as long as at least 2 coins have been inserted.  Responds with 200, X-Coins, X-Items-Remaining, and the number of items vended if there were sufficient funds and the item was in stock.  Responds with 404 and X-Coins if the item was out of stock.  Responds with 403 and X-Coins if the machine requires more money to dispense the item.

### DELETE
* **/** 
    * Returns X-Coins that were inserted to be refunded.
