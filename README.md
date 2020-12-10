# EZMongo.NET
## What is EZMongo.NET
EZMongo is a class library that allows .NET developers to implement MongoDB Create, Read, Update and Delete (CRUD) operations against a single collection. 
With methods for CRUD operations, just provide information (connection string and name of the collection) when creating a new instance and you are good to go.

## How it works?
EZMongo.NET has 5 methods performing the basic CRUD operations for a single collection and a method for getting the total row count of a single collection.

Methods include:


* Create(string, object) - Inserts a certain item into the collection. The first parameter references the name of the collection to insert to and the second parameter is the item to be inserted into the collection. 
The second parameter takes an object type that contains the deserialized data from a JSON stream.
* Read(string) - Returns the contents of a single collection. The parameter references the name of the collection.
* Read(string, Dictionary<object,object>) - Returns the contents of a single collection with respect to the filters. 
The filters take the form of a key-value pair wherein the column name is the key and the value to be filtered is the value
* Update(string, object, Dictionary<object,object>) - updates an item in the collection with respect to the filters. The second parameter  takes an object type that contains the deserialized data from a JSON stream which contains the updated value
* Delete(string, string, string) - deletes an item in the collection. The first parameter references the name of the collection, second parameter is the name of the column to be referenced, and the third parameter is the value in that column to be deleted.
* Count(string) - returns the total row count of a single collection.


## Using the code

### Creating a new instance
EZMongo.NET has a class called Client that implements the IDisposable interface which means that the `using` keyword can be used to create a new instance.

```cs
using (Client client = new Client(connStr, database))
 {
     //code here
 }
 ```
 The connection strings can be either from a local MongoDB installed from a development machine or from a Platform-As-A-Service (PAAS) such as MongoDB Atlas.
 
 ## Limitations
 This code currently supports CRUD operations for one single collection. This currently has no validation checks if the connection string supplied is for a MongoDB connection.
 
