# EFCAT
A simple tool for your entity framework core project.

## Usage
First of all, to implement EFCAT you need to change the dependy of your DBContext from DbContext to DatabaseContext.
After that the new attributes get registered by the Entity Framework.

[PrimaryKey]
This attribute makes your property to a primary key.

[ForeignColumn(TYPE, KEYS)]
