# Auction Application

Technology Stack:

* C#
* Asp .NET MVC
* Asp .NET Web API
* Amazon DynamoDB
* Amazon SQS


### Overview

**Auction Rooms** - there can be one or more auction rooms in the auction house. An auction room contains a set of auction items that the user can bid on. 
Once an auction room gets active, the contained auction items are available for auctioning, one at a time. 


**Auction items** - an auction room contains one or more items. These items are available for bidding one at a time( after the auction is finished for the current item, auctioning will start on next item **ready** for auction).

An item can be in one of the following states:

 * ready
 * active
 * finished

**Active item** - after the auction has started, the auction room will contain one and only one auction item with `state=active`. Users can place a bid on the item that's currently active.

**Bidding** - the user can bid on the current **active item**. 
The auction starts from the **starting_price** associated to the the auction item. 
The bids are incremented starting from the current offered price for the item.
When the user submits a bid, the request is posted on the bid queue. The requests are read from the queue, processed and the current status of the auctioned item is updated in the database.

**Winning an auctioned item** - after the auction is finished on the current active item, the user that placed the highest bid wins the auctioned item.

A timespan is configured as a wait time interval from the time the last bid was submitted. If this timespan has elapsed from the last bid, the user that made the last bid wins the auction for the current active item. 

### Data Persistence

The data regarding the auction rooms and items, the users and the results of the auctions are persisted in the DynamoDB database.

-----

### Projects Overview

**Auctionata.Mvc** - the web client for the auction application. Allows the user to view the auction rooms, to bid if the auction room is active and if the user is registered to that aution. The web client communicates with the Web Api Services defined in **Auctionata.Api**

**Auctionata.Api** - contains 3 web API services:

 * Auction
 * Auctions
 * Users
 
* `GET api/users` - retrieve list of users
* `GET api/users/{code}`- retrieve list of users registered to the auction with the given auction code
* `POST api/users json` - register user to auction

Example of json for `POST api/users json`:

`{"AuctionCode":"NeedForSpeed","Email":"test@gmail.com"}`

`GET api/auctions` - get auction list
`GET api/auctions/{code}` - get auction by code
`GET api/auctions/{code}?onlyActive=true` - get only active items for auction with given code
`GET api/auctions/{code}?onlyActive=false` - get all items for auction with given code

* `POST api/auction` - for placing a bid
Example of json:
`{"User":"test@gmail.com","Amount":120.0,"AuctionCode":"NeedForSpeed"}`
 
**Auctionata.Runner** - console application that processes the messages arrived in the queue. It reads the messages and processes them every 1 second and updates the current status of the auction in the database.


### Database Schema

##### DynamoDB database

![DynamoDB database](/img/DynamoDB.JPG)

##### Users 
![Users](/img/Users.JPG)

##### Auctions
![Auctions](/img/Auctions.JPG)

##### Items
![Items](/img/Items.JPG)

##### Bid Queue
![SQS QUEUE](/img/SQSQueue.JPG)

-----

### Testing the Application

Start the web API services (`Auctionata.Api`).

Before starting the web client(`Auctionata.Mvc`) make adjustments on the port that the web API services are running ( for the moment they are hardcoded, this will need to be refactored). So the URLs for the web API services need to be fixed.

Start the Web client. You are able to view the auction rooms and register to them.

To start the auctioning on one room, we must start the queue processing. Start the `Auctionata.Runner` console application with an argument that corresponds to the auction we want to start( e.g.: NeedForSpeed)

Now you are able to bid on the items from that auction room, if you are registered in the auction.

You can see below a screenshot for an auction room, before the user registered.

![Auction Room](/img/mvc_auction_unregistred.JPG)

Below you can see a screenshot for an auction room that's currently active and for which the user is registered to.

![Auction Room](/img/mvc_started_auction.JPG)




#### Reset auctions

To be able to restart testing, run `ResetAuctionConsole`, that will reset the status for each auction room.

