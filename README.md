# Auction Application

Technology Stack:

* C#
* Asp .NET MVC
* Asp .NET Web API
* Amazon DynamoDB
* Amazon SQS


### Overview

**Auction Rooms** - the user is able to see the details regarding the auction rooms and the related items. The user is able to bid on the current auctioned item if he is registered to the auction.
Once the auction room gets active( one contained at a time is active for auctioning) the user is able to bid and to see the details regarding the last bid value and the user that submitted it. 

**Auction items** - an auction room contains one or more items. These items are available for bidding one at a time( after the auction is finished for the current item, auctioning will start on next item **ready** for auction).

An item can be in one of the following states:

 * ready
 * active
 * finished

**Active item** - the item that's currently being auctioned.
 An auction room can contain only one item with state=active at a time.

**Bidding** - the user can bid on the current **active item**. The bids are incremented starting from the **starting price**. When bidding, the request for the current bid value is sent on the bid queue. The request that gets first in the queue will be considered winner for the current value of the bid.

**Winning an auctioned item** - a timespan is configured as a wait time from the time the last bid was submitted. If this timespan has elapsed from the last bid and no other request to bid with an incremented value was submitted, the user that made the last bid wins the auction for the current active item. 

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
