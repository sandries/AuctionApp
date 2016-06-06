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

The data regarding the auction rooms and items, the user and the results of the auction are persisted in the DynamoDB database.

-----

### Projects Overview

**Auctionata.Mvc** - the web client for the auction application. Allows the user to view the auction rooms, to bid if the auction room is active and if the user is registered to that aution. The web client communicates with the Web Api Services defined in **Auctionata.Api**

**Auctionata.Api** - contains 3 web API services that:

 * manage users
 * get auction rooms and items
 * bid on active item
 
**Auctionata.Runner** - console application that processes the messages arrived in the queue. It reads the messages and processes them every 1 second and updates the current status of the auction in the database.

-----

### Testing the Application
