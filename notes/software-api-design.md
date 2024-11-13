# Design

"Software Has"...

- A Vendor
- A Name
- A Description
- Identity of Software Center employee that added the item.
- Date and time it was added.

## Step 1 - What is this doing? What are we doing? Who are these people? THIS IS NOT MY BEAUTIFUL WIFE... (The "Operation")
- A business process that has a "Side Effect" (just a mathy way of saying something about our world will change because of this.)
- A post to a collection of some type. So a plural URI?
- The operation "Add a piece of software to the catalog"

## Step 2 - What data do we need to do this? Where does it come from? (The "Operands")

- Some operands are "references" to data we do not own.
    - Identity (Authorization Header) is always data we don't own.
    - Vendor is a reference to an existing vendor (maybe our API has this, maybe it doesn't)
    - Date and Time - reference to the clock when this was added. (maybe using a shared clock service, time is hard)
- Some are data that we do or will own.
    - Name of the software
    - Description


## Step 3 - Map this to HTTP
- Preferring HTTP "Knowns" - status codes, etc.



### Bad Take

```http
POST /vendors/7473ee24-54d2-48f4-8e84-d240d65e4b16/catalog
Content-Type: application/json
Authorization: bearer (with a JWT that says they are in the role of "software-center")

{
 
    "vendorid": "893898938983983"
    "name": "Visual Studio Code",
    "description": "An editor for programmers"
  
}
```
400 - Bad Request
    - The most passive aggressive BS you can send to a client.
