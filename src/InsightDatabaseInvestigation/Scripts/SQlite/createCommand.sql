CREATE TABLE UserGroup
(
    GroupID INTEGER PRIMARY KEY AUTOINCREMENT,
    Name varchar(100) not null,
    Comment varchar(1000)
);

CREATE TABLE [User]
(
	UserID INTEGER PRIMARY KEY AUTOINCREMENT,
	FirstName varchar(100) not null,
	LastName varchar(100) not null,
	Middle varchar(1) not null,
	Email varchar(100) not null,
	Phone varchar(16) not null,
	Comment varchar(1000)
);

CREATE TABLE Membership
(
	MembershipID INTEGER PRIMARY KEY AUTOINCREMENT,
	GroupID integer not null,
	UserID integer not null,
	FOREIGN KEY(GroupID) references UserGroup(GroupID),
	FOREIGN KEY(UserID) references [User](SomeID)
);