-- Supplies
create table SuppliesTransaction(
SuppliesTransactionID varchar(7) primary key,
SuppliesTransactionDate datetime default getdate(),
SuppliesTransactionStatus varchar(25) default 'Uncheck',
SuppliesTransactionDescription varchar(100) default 'No Description',
RequestBy varchar(7),
UpdatedBy varchar(7),
--constraint constSTR check (SuppliesTransactionID like 'STR[0-9][0-9][0-9][0-9]')
)
create table DetailSuppliesTransaction(
SuppliesTransactionID varchar(7),
MilkID varchar(7),
Quantity int,
Price int,
VendorID varchar(7),
primary key(SuppliesTransactionID, MilkID)
)
create table Milk(
MilkID varchar(7) primary key,
MilkName varchar(25),
MilkInputDate datetime default getdate(),
InputBy varchar(7),
MilkLifeTime date default dateadd(day,365,getdate()),
--constraint constMLK check (MilkID like 'MLK[0-9][0-9][0-9][0-9]')
)
create table Cheese(
CheeseID varchar(7) primary key,
CheeseName varchar(25),
CheeseInputDate datetime default getdate(),
InputedBy varchar(7),
CheeseLifeTime date default dateadd(day,365,getdate()),
--constraint constCHS check (CheeseID like 'CHS[0-9][0-9][0-9][0-9]')
)
create table Warehouse(
WarehouseID varchar(7) primary key,
UpdatedDate datetime default getdate(),
UpdatedBy varchar(7),
--constraint constWRH check (WarehouseID like 'WRH[0-9][0-9][0-9][0-9]')
)
create table DetailWarehouse(
WarehouseID varchar(7),
ProductID varchar(7),
Quantity int,
Status varchar(MAX),
primary key (WarehouseID, ProductID)
)
create table Vendor(
VendorID varchar(7) primary key,
VendorName varchar(25),
VendorRegisterDate datetime default getdate(),
VendorRegisterBy varchar(7),
--constraint constVDR check (VendorID like 'VDR[0-9][0-9][0-9][0-9]')
)

-- Incoming Supplies
create table GoodsReceipt(
GoodsReceiptID varchar(7) primary key,
SuppliesTransactionID varchar(7),
GoodsReceiptDate datetime default getdate(),
GoodsReceiptDescription varchar(100) default 'No Description',
CreatedBy varchar(7),
--constraint constGRD check (GoodsReceiptID like 'GRD[0-9][0-9][0-9][0-9]')
)
create table DetailGoodsReceipt(
GoodsReceiptID varchar(7),
MilkID varchar(7),
Quantity int,
Price int,
VendorID varchar(7),
primary key (GoodsReceiptID, MilkID)
)
create table FinalGoodsReceipt(
FinalGoodsReceiptID varchar(7) primary key,
GoodsReceiptCheckID varchar(7),
FinalGoodsReceiptDate datetime default getdate(),
CreatedBy varchar(7),
--constraint constFGR check (FinalGoodsReceiptID like 'FGR[0-9][0-9][0-9][0-9]')
)
create table DetailFinalGoodsReceipt(
FinalGoodsReceiptID varchar(7) ,
MilkID varchar(7),
Quantity int,
Price int,
VendorID varchar(7),
primary key (FinalGoodsReceiptID, MilkID)
)
create table VerifiedInvoice(
VerifiedInvoiceID varchar(7) primary key,
FinalGoodsReceiptID varchar(7),
SuppliesTransactionID varchar(7),
VerifiedInvoiceDate datetime default getdate(),
CreatedBy varchar(7),
VerifiedInvoiceStatus varchar(25) default 'Waiting for Approval',
ApprovedBy varchar(7) default null,
ApprovedDate datetime default null,
PaidBy varchar(7) default null,
PaidConfirmationDate datetime default null,
TotalPrice int,
--constraint constVFI check (VerifiedInvoiceID like 'VFI[0-9][0-9][0-9][0-9]')
)

-- Quality Management
create table GoodsReceiptCheck(
GoodsReceiptCheckID varchar(7) primary key,
GoodsReceiptID varchar(7),
GoodsReceiptCheckDate datetime default getdate(),
CreatedBy varchar(7)
)
create table DetailGoodsReceiptCheck(
GoodsReceiptCheckID varchar(7),
MilkID varchar(7),
Quantity int,
Price int,
VendorID varchar(7),
primary key (GoodsReceiptCheckID, MilkID)
)
create table DefectiveProduct(
DefectiveProduct varchar(7) primary key,
--constraint constDFP check (DefectiveProduct like 'DFP[0-9][0-9][0-9][0-9]')
)

-- Finance
 create table SalesInvoice (
 SalesInvoiceID varchar(7) primary key,
 --constraint constSLI check (SalesInvoiceID like 'SLI[0-9][0-9][0-9][0-9]')
 )


-- Sales & Distribution
create table Customer(
CustomerID varchar(7) primary key,
CustomerName varchar(25),
CustomerGender varchar(10),
CustomerAddress varchar(100),
CustomerContactNumber varchar (25),
CustomerEmail varchar (50),
RegisterDate datetime default getdate(),
RegisterBy varchar(7)
)
create table DetailCustomer(
CustomerID varchar(7),
LastUpdatedBy varchar(7),
LastUpdatedDate datetime default getdate(),
primary key (CustomerID, LastUpdatedDate)

)
create table SalesTransaction(
SalesTransactionID varchar(7) primary key,
CustomerID varchar(7),
SalesTransactionDate datetime default getdate(),
PaymentType varchar(25),
--SoldToParty varchar(25),
ShippingLocation varchar(50),
Transportation varchar(50),
ShippingTime datetime default dateadd(day,7,getdate()),
NumberOfBatch int,
SalesTransactionStatus varchar(25) default '',
InsertBy varchar(7),
InsertDate datetime default getdate(),
ConfirmBy varchar(7) default null,
ConfirmDate datetime default null,
TransportPrice int,
--constraint constSLT check (SalesTransactionID like 'SLT[0-9][0-9][0-9][0-9]')
)
create table DetailSalesTransaction(
SalesTransactionID varchar(7),
CheeseID varchar(7),
Quantity int,
ItemPrice int,
TotalPrice int
primary key (SalesTransactionID, CheeseID)
)
create table Schedule(
--ScheduleID varchar(7) primary key,
ScheduleID varchar(7),
SalesTransactionID varchar(7),
BatchNumberSchedule int,
PackingDate date default null,
ShippingDate date default null,
ScheduleStatus varchar(25) default 'Not Done',
--constraint constSCH check (ScheduleID like 'SCH[0-9][0-9][0-9][0-9]')
primary key (ScheduleID, SalesTransactionID)
)

-- Outgoing Products
create table OutgoingReport(
OutgoingReportID varchar(7) primary key,
SalesTransactionID varchar(7),
ScheduleID varchar(7),
CheckStatus varchar(25) default 'Waiting For Approval',
DeliveryStatus varchar(25) default 'Unsend',
PackedBy varchar(7),
ShippedBy varchar(7) default null,
OutgoingReportDescription varchar(100) default 'No Description',
CheckedBy varchar(7) default null,
CheckedDate datetime default null,
--constraint constOGP check (OutgoingReportID like 'OGP[0-9][0-9][0-9][0-9]')
)

-- Human Resources
create table Employee(
EmployeeID varchar(7) primary key, 
EmployeeName varchar(25),
EmployeeGender varchar(25),
Division varchar(25),
Salary int,
DateOfBirth date,
JoinDate datetime default getdate(),
EmployeeEmail varchar(50),
PhoneNumber varchar(25),
RegisteredBy varchar(7),
EmployeeStatus varchar(25) default 'Active',
)

create table DetailEmployee(
EmployeeID varchar(7),
LastUpdatedBy varchar(7),
LastUpdateDate datetime default getdate(),
primary key (EmployeeID, LastUpdateDate)
)
