# Auction Application

Technology Stack:

* C#
* Asp .NET MVC
* Asp .NET Web API
* Amazon DynamoDB
* Amazon SQS


### Overview

**Auctionata.Mvc** it's the web client that allows us to view the auction rooms and items and to register for an auction.
Once an auction room gets active( items are available to be auctioned) the user is able to bid for the current item if he registered to the auction.

**Active item** - an auction room contains one or more items. These items are available for bidding one at a time( after the auction is finished for the current item, auctioning will start on next item **ready** for auction).

An item can be in one of the following states:

 * ready
 * active
 * finished
 
An auction room can contain only one item with state=active at a time.

**Bidding** - the user can bid on the current **active item**. The bids are incremented starting from the **starting price**. When bidding, the request for the current bid value is sent on the bid queue. The request that gets first in the queue will be considered winner for the current value of the bid.

**Winning an auctioned item** - a timespan is configured as a wait time from the time the last bid was submitted. If this timespan has elapsed from the last bid and no other request to bid with an incremented value was submitted, the user that made the last bid wins the auction for the current active item. 

The data regarding the auction rooms and items, the user and the results of the auction are persisted in the DynamoDB database.
