

create table Role(
		roleID int  identity(1,1) not null primary key,
		roleName nvarchar(255) not null
);

create table UserInfo (
	userID int NOT NULL  identity(1,1) primary key,
	username nvarchar(255) NOT NULL,
	passwordEncrypted nvarchar(255) NOT NULL,
	fullName nvarchar(255) NOT NULL,
	phoneNumber nvarchar(10) NOT NULL,
	roleID int NOT NULL,
	foreign key (roleID) references Role(roleID)
);



create table BeverageType(
	typeID int  identity(1,1) not null primary key,
	typeName nvarchar(100) not null,
)

create table Customer(
	customerID int  identity(1,1) not null primary key,
	customerName nvarchar(255) not null,
	phone nvarchar(255) not null,
	point float default 0,
)

create table Beverage(
	beverageID int  identity(1,1) not null primary key,
	beverageName nvarchar(255) not null,
	beveragePrice float not null,
	beverageImage varbinary(max) not null,
	typeID int not null,
	foreign key (typeID) references BeverageType(typeID)
)

create table Voucher(
	voucherID nvarchar(255) not null primary key,
	voucherEndDate DateTime not null,
	voucherValue float not null,

	check (voucherValue <=1 and voucherValue>0),
)

create table Receipt(
	receiptID int  identity(1,1) not null primary key,
	dateCreated DateTime not null,
	receiptValue float not null,
	customerID int not null references Customer(customerID),
	userID int not null references UserInfo(userID),
	voucherID nvarchar(255) not null references Voucher(voucherID),
)

create table ReceiptDetail(
	receiptID int not null references Receipt(receiptID),
	beverageID int not null references Beverage(beverageID),
	quantity int not null,
	primary key(receiptID,beverageID) 
)
create Table Unit(
	unitID int  identity(1,1) not null primary key,
	unitName nvarchar(100) not null,
)

create table Ingredient(
	ingredientID int  identity(1,1) not null primary key,
	ingredientName nvarchar(255) not null,
	ingredientPrice float not null,
	unitID int not null references Unit(unitID),
)
create table PaymentVoucher(
	paymentID int  identity(1,1) not null primary key,
	dateCreated DateTime not null,
	paymentValue float not null,
	userID int not null references UserInfo(userID)
)
create table PaymentDetail(
	paymentID int not null references PaymentVoucher(paymentID),
	ingredientID int not null references Ingredient(ingredientID),
	ingredientQuantity int not null,
	primary key(paymentID,ingredientID) 
)

select * from BeverageType
select * from Receipt where customerID=29

select * from Ingredient
select * from Unit

select * from UserInfo

select * from Customer

delete from Customer where customerID=29

