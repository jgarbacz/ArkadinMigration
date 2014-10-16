-- ####### NEW DB OBJECTS
/*CREATE SCHEMA mcTesting AUTHORIZATION dbo;
GO*/

-- Utility functions
/*IF OBJECT_ID('mcTesting.timGetNums', 'IF') IS NOT NULL DROP FUNCTION mcTesting.timGetNums;
GO
CREATE FUNCTION mcTesting.timGetNums(@low AS BIGINT, @high AS BIGINT) RETURNS TABLE
AS
RETURN
WITH
L0 AS (SELECT c FROM (VALUES(1),(1)) AS D(c)),
L1 AS (SELECT 1 AS c FROM L0 AS A CROSS JOIN L0 AS B),
L2 AS (SELECT 1 AS c FROM L1 AS A CROSS JOIN L1 AS B),
L3 AS (SELECT 1 AS c FROM L2 AS A CROSS JOIN L2 AS B),
L4 AS (SELECT 1 AS c FROM L3 AS A CROSS JOIN L3 AS B),
L5 AS (SELECT 1 AS c FROM L4 AS A CROSS JOIN L4 AS B),
Nums AS (SELECT ROW_NUMBER() OVER(ORDER BY (SELECT NULL)) AS rownum
FROM L5)
SELECT @low + rownum - 1 AS n
FROM Nums
WHERE rownum BETWEEN @low AND @high
GO

IF OBJECT_ID('mcTesting.timGetEnum', 'FN') IS NOT NULL DROP FUNCTION mcTesting.timGetEnum;
GO
CREATE FUNCTION mcTesting.timGetEnum (@EnumNS nvarchar(255), @Enum nvarchar(255), @iCount int)
RETURNS nvarchar(255)
AS
BEGIN
  DECLARE @x nvarchar(255);
  SELECT @x = Val 
  FROM mcTesting.Enums AS E
  INNER JOIN (
    SELECT EnumNS, Enum, ((@iCount % Cnt) + 1) AS Idx
    FROM mcTesting.EnumHeader
    WHERE EnumNS = @EnumNS
    AND Enum = @Enum
  ) AS H ON H.EnumNS = E.EnumNS AND H.Enum = E.Enum AND H.Idx = E.Rnk
  WHERE E.EnumNS = @EnumNS
  AND E.Enum = @Enum 
  --AND E.Rnk = @iCount --((@iCount % H.Cnt) + 1);
  RETURN @x;
END;

IF OBJECT_ID('mcTesting.GetEnumBracket', 'IF') IS NOT NULL DROP FUNCTION mcTesting.GetEnumBracket;
GO
CREATE FUNCTION mcTesting.GetEnumBracket(@start AS int, @end AS int, @Enum AS nvarchar(125), @EnumNS AS nvarchar(255)) RETURNS TABLE
AS
RETURN
SELECT foo.n, E.EnumNS, E.Enum, E.Val, E.Rnk 
FROM mcTesting.Enums AS E
INNER JOIN (
    select N.n, H.EnumNS, H.Enum, H.Cnt, ((N.n % H.Cnt) + 1) AS RecNum 
    from mcTesting.timGetNums(@start,@end) AS N
    cross join mcTesting.EnumHeader AS H
) AS foo ON foo.Enum = E.Enum AND foo.EnumNS = E.EnumNS AND E.Rnk = foo.RecNum
WHERE foo.Enum = @Enum
AND foo.EnumNS = @EnumNS
GO*/

SET NOCOUNT ON;

-- clean up
IF EXISTS(SELECT * FROM sys.tables WHERE name = 'Enums' AND type = N'U')
  DROP TABLE mcTesting.Enums;
IF EXISTS(SELECT * FROM sys.tables WHERE name = 'EnumHeader' AND type = N'U')
  DROP TABLE mcTesting.EnumHeader;
IF EXISTS(SELECT * FROM sys.tables WHERE name = 'SiloAcct' AND type = N'U')
  DROP TABLE mcTesting.SiloAcct;
IF EXISTS(SELECT * FROM sys.tables WHERE name LIKE 'PartnerAcct' AND type = N'U')
  DROP TABLE mcTesting.PartnerAcct;
IF EXISTS(SELECT * FROM sys.tables WHERE name LIKE 'LogoAcct' AND type = N'U')
  DROP TABLE mcTesting.LogoAcct;
IF EXISTS(SELECT * FROM sys.tables WHERE name LIKE 'LocalLogoAcct' AND type = N'U')
  DROP TABLE mcTesting.LocalLogoAcct;
IF EXISTS(SELECT * FROM sys.tables WHERE name LIKE 'LegalEntityAcct' AND type = N'U')
  DROP TABLE mcTesting.LegalEntityAcct;
IF EXISTS(SELECT * FROM sys.tables WHERE name LIKE 'RepGrpOneAcct' AND type = N'U')
  DROP TABLE mcTesting.RepGrpOneAcct;
IF EXISTS(SELECT * FROM sys.tables WHERE name LIKE 'RepGrpTwoAcct' AND type = N'U')
  DROP TABLE mcTesting.RepGrpTwoAcct;
IF EXISTS(SELECT * FROM sys.tables WHERE name LIKE 'BillingAcct' AND type = N'U')
  DROP TABLE mcTesting.BillingAcct;
IF EXISTS(SELECT * FROM sys.tables WHERE name LIKE 'GroupAcct' AND type = N'U')
  DROP TABLE mcTesting.GroupAcct;
IF EXISTS(SELECT * FROM sys.tables WHERE name LIKE 'PrimaryGroupAcct' AND type = N'U')
  DROP TABLE mcTesting.PrimaryGroupAcct;
IF EXISTS(SELECT * FROM sys.tables WHERE name LIKE 'UserAcct' AND type = N'U')
  DROP TABLE mcTesting.UserAcct;
IF EXISTS(SELECT * FROM sys.tables WHERE name LIKE 'AccessAcct' AND type = N'U')
  DROP TABLE mcTesting.AccessAcct;
IF EXISTS(SELECT * FROM sys.tables WHERE name LIKE 'AcctContact' AND type = N'U')
  DROP TABLE mcTesting.AcctContact;
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_Enums_EnumNSEnum') 
    DROP INDEX IX_Enums_EnumNSEnum ON mcTesting.Enums; 
IF EXISTS(SELECT * FROM sys.tables WHERE name = 'AcctName' AND type = N'U')
  DROP TABLE mcTesting.AcctName;
IF EXISTS(SELECT * FROM sys.tables WHERE name LIKE 'Names' AND type = N'U')
  DROP TABLE mcTesting.Names;
IF EXISTS(SELECT * FROM sys.tables WHERE name LIKE 'Addresses' AND type = N'U')
  DROP TABLE mcTesting.Addresses;

-- Tables to hold enum values
CREATE TABLE mcTesting.Enums (EnumFull nvarchar(255), EnumNS nvarchar(255), Enum nvarchar(125), Val nvarchar(255), Rnk int, PRIMARY KEY(EnumFull));
CREATE TABLE mcTesting.EnumHeader (EnumNS nvarchar(255), Enum nvarchar(125), Cnt int, PRIMARY KEY(EnumNS,Enum));
CREATE NONCLUSTERED INDEX IX_Enums_EnumNSEnum ON mcTesting.Enums (EnumNS,Enum); 

-- Table to hold account names
CREATE TABLE mcTesting.AcctName (ID int, LegacyAccountId varchar(255), AccountTypeID int, PRIMARY KEY(ID,AccountTypeID));

-- Tables to hold account values
CREATE TABLE mcTesting.SiloAcct (LegacyAccountId varchar(255) NOT NULL, LegacySystem varchar(50) NOT NULL, StartDate smalldatetime NOT NULL, 
  EndDate smalldatetime NULL, AccountStatus varchar(2) NOT NULL, CustomerNodeName varchar(255) NOT NULL, IsMigrated char(1));
CREATE TABLE mcTesting.PartnerAcct (LegacyAccountId varchar(255) NOT NULL, LegacySystem varchar(50) NOT NULL, ParentLegacyAccountId varchar(255) NULL, 
  PayerLegacyAccountId varchar(255) NULL, StartDate smalldatetime NOT NULL, EndDate smalldatetime NULL, AccountStatus varchar(2) NOT NULL, 
  AccountPassword varchar(50) NOT NULL, CreatedBy varchar(50) NOT NULL, CreatedDate smalldatetime NOT NULL, ModifiedBy varchar(50) NOT NULL, 
  ModifiedDate smalldatetime NOT NULL, OrderStatus nvarchar(255) NOT NULL, CustPartnerExtId varchar(50) NULL, CustomerNodeName varchar(255) NOT NULL, 
  CommentC varchar(255) NULL, Language nvarchar(255) NOT NULL, Currency nvarchar(255) NOT NULL, Timezone nvarchar(255) NOT NULL, 
  SecurityQuestion nvarchar(255) NOT NULL, SecurityAnswer varchar(255) NULL, UsageCycleType nvarchar(255) NOT NULL, 
  PaymentMethod nvarchar(255) NOT NULL, RegistrationNumber varchar(50) NULL, SalesforceIDB varchar(15) NOT NULL, SubBillingB nvarchar(255) NOT NULL, 
  SubManagementB nvarchar(255) NULL, SubRevenueB nvarchar(255) NULL, NavisionInstance nvarchar(255) NOT NULL, PaymentTermCode nvarchar(255) NOT NULL, 
  InvoiceDeliveryMethod nvarchar(255) NOT NULL, InvoiceOutputFormat nvarchar(255) NOT NULL, InvoiceLanguage nvarchar(255) NOT NULL, 
  InvoiceMinimumAmount decimal NOT NULL, LegalEntityInheritance nvarchar(255) NOT NULL, CentralBillingFlagBA bit NOT NULL, CUDTemplate nvarchar(255) NOT NULL, 
  EUDTemplate nvarchar(255) NOT NULL, IsCoverPageRequired bit NOT NULL, PurchaseOrderReference varchar(20) NULL, IsPurchaseOrderMandatory bit NOT NULL, 
  PurchaseOrderExpiryDate date NULL, ArkadinBankAccountDetailsId varchar(20) NOT NULL, PrintHouse nvarchar(255) NOT NULL, PartnerRelationshipType nvarchar(255) NOT NULL, 
  PartnershipStartDate smalldatetime NOT NULL, PartnershipEndDate smalldatetime NULL, CRMonBehalfOf bit NULL, ARonBehalfOf bit NULL, MarketingonBehalfOf bit NULL, 
  PartnerRegion varchar(50) NULL, TaxExemptID varchar(255) NULL, TaxVendor nvarchar(255) NULL, MetraTaxCountryEligibility nvarchar(255) NULL, 
  MetraTaxCountryZone nvarchar(255) NULL, MetraTaxHasOverrideBand bit NULL, MetraTaxOverrideBand nvarchar(255) NULL, TaxServiceAddressPCode integer NULL, 
  TaxExemptReason nvarchar(255) NULL, TaxExemptionStartDate smalldatetime NULL, TaxExemptionEndDate smalldatetime NULL, TaxRegistryReference varchar(255) NULL, 
  BilledThroughDate smalldatetime NOT NULL, IsMigrated char(1));

CREATE TABLE mcTesting.LogoAcct (LegacyAccountId varchar(255) NOT NULL,LegacySystem varchar(50) NOT NULL,ParentLegacyAccountId varchar(255) NULL,
  PayerLegacyAccountId varchar(255) NULL,StartDate smalldatetime NOT NULL,EndDate smalldatetime NULL,AccountStatus varchar(2) NOT NULL,
  CreatedBy varchar(50) NOT NULL,CreatedDate smalldatetime NOT NULL,ModifiedBy varchar(50) NOT NULL,ModifiedDate smalldatetime NOT NULL,
  OrderStatus nvarchar(255) NOT NULL,CustPartnerExtId varchar(50) NULL,CustomerNodeName varchar(255) NOT NULL,CommentC varchar(255) NULL,
  SubManagementL nvarchar(255) NOT NULL,CentralBillingFlagL bit NOT NULL,EffectiveMgmtSubsidiary nvarchar(255) NULL, IsMigrated char(1));

CREATE TABLE mcTesting.LocalLogoAcct (LegacyAccountId varchar(255) NOT NULL,LegacySystem varchar(50) NOT NULL,ParentLegacyAccountId varchar(255) NULL,
  PayerLegacyAccountId varchar(255) NULL,StartDate smalldatetime NOT NULL,EndDate smalldatetime NULL,AccountStatus varchar(2) NOT NULL,
  CreatedBy varchar(50) NOT NULL, CreatedDate smalldatetime NOT NULL, ModifiedBy varchar(50) NOT NULL, ModifiedDate smalldatetime NOT NULL,
  OrderStatus nvarchar(255) NOT NULL, CustPartnerExtId varchar(50) NULL,CustomerNodeName varchar(255) NOT NULL,CommentC varchar(255) NULL,
  SubManagementLL nvarchar(255) NULL, EffectiveMgmtSubsidiary nvarchar(255) NULL, LogoUserName varchar(255) NOT NULL, IsMigrated char(1));

CREATE TABLE mcTesting.LegalEntityAcct (LegacyAccountId varchar(255) NOT NULL,LegacySystem varchar(50) NOT NULL,ParentLegacyAccountId varchar(255) NULL,
  PayerLegacyAccountId varchar(255) NULL,StartDate smalldatetime NOT NULL,EndDate smalldatetime NULL,AccountStatus varchar(2) NOT NULL,
  CreatedBy varchar(50) NOT NULL,CreatedDate smalldatetime NOT NULL,ModifiedBy varchar(50) NOT NULL,ModifiedDate smalldatetime NOT NULL,
  OrderStatus nvarchar(255) NOT NULL,CustPartnerExtId varchar(50) NULL,CustomerNodeName varchar(255) NOT NULL,CommentC varchar(255) NULL,
  SubBilling nvarchar(255) NOT NULL,PaymentTermLE nvarchar(255) NOT NULL,CommercialRegistrationNumber varchar(20) NULL,
  TaxRegistrationNumber varchar(20) NULL,BillingCurrency varchar(20) NOT NULL,EffectiveMgmtSubsidiary nvarchar(255) NULL,
  LogoUserName varchar(255) NOT NULL);

CREATE TABLE mcTesting.RepGrpOneAcct (LegacyAccountId varchar(255) NOT NULL,LegacySystem varchar(50) NOT NULL,ParentLegacyAccountId varchar(255) NULL,
  PayerLegacyAccountId varchar(255) NULL,StartDate smalldatetime NOT NULL,EndDate smalldatetime NULL,AccountStatus varchar(2) NOT NULL,
  CreatedBy varchar(50) NOT NULL,CreatedDate smalldatetime NOT NULL,ModifiedBy varchar(50) NOT NULL,ModifiedDate smalldatetime NOT NULL,
  OrderStatus nvarchar(255) NOT NULL,CustPartnerExtId varchar(50) NULL,CustomerNodeName varchar(255) NOT NULL,CommentC varchar(255) NULL, 
  EffectiveMgmtSubsidiary nvarchar(255) NULL,LogoUserName varchar(255) NOT NULL, IsMigrated char(1));

CREATE TABLE mcTesting.RepGrpTwoAcct (LegacyAccountId varchar(255) NOT NULL,LegacySystem varchar(50) NOT NULL,ParentLegacyAccountId varchar(255) NULL,
  PayerLegacyAccountId varchar(255) NULL,StartDate smalldatetime NOT NULL,EndDate smalldatetime NULL,AccountStatus varchar(2) NOT NULL,
  CreatedBy varchar(50) NOT NULL,CreatedDate smalldatetime NOT NULL,ModifiedBy varchar(50) NOT NULL,ModifiedDate smalldatetime NOT NULL,
  OrderStatus nvarchar(255) NOT NULL,CustPartnerExtId varchar(50) NULL,CustomerNodeName varchar(255) NOT NULL,CommentC varchar(255) NULL, 
  EffectiveMgmtSubsidiary nvarchar(255) NULL,LogoUserName varchar(255) NOT NULL, IsMigrated char(1));

CREATE TABLE mcTesting.BillingAcct (LegacyAccountId varchar(255) NOT NULL,LegacySystem varchar(50) NOT NULL,ParentLegacyAccountId varchar(255) NULL,
  PayerLegacyAccountId varchar(255) NULL,StartDate smalldatetime NOT NULL,EndDate smalldatetime NULL,AccountStatus varchar(2) NOT NULL,
  AccountPassword varchar(50) NOT NULL,CreatedBy varchar(50) NOT NULL,CreatedDate smalldatetime NOT NULL,ModifiedBy varchar(50) NOT NULL,ModifiedDate smalldatetime NOT NULL,
  OrderStatus nvarchar(255) NOT NULL,CustPartnerExtId varchar(50) NULL,CustomerNodeName varchar(255) NOT NULL,CommentC varchar(255) NULL,
  [Language] nvarchar(255) NOT NULL,Currency varchar(10) NOT NULL,Timezone nvarchar(255) NOT NULL,SecurityQuestion nvarchar(255) NOT NULL,
  SecurityAnswer varchar(255) NULL,UsageCycleType nvarchar(255) NOT NULL,PaymentMethod nvarchar(255) NOT NULL, RegistrationNumber varchar(50) NULL,
  SalesforceIDB varchar(15) NOT NULL, SubBillingB nvarchar(255) NOT NULL,SubManagementB nvarchar(255) NULL,SubRevenueB nvarchar(255) NULL,
  NavisionInstance nvarchar(255) NOT NULL,PaymentTermCode nvarchar(255) NOT NULL,InvoiceDeliveryMethod nvarchar(255) NOT NULL,
  InvoiceOutputFormat nvarchar(255) NOT NULL,InvoiceLanguage nvarchar(255) NOT NULL,InvoiceMinimumAmount decimal(22,10) NOT NULL,
  MasterLegalEntity varchar(255) NULL,LegalEntityInheritance nvarchar(255) NOT NULL,CentralBillingFlagBA bit NOT NULL,
  CUDTemplate nvarchar(255) NOT NULL,EUDTemplate nvarchar(255) NOT NULL,IsCoverPageRequired bit NOT NULL,PurchaseOrderReference varchar(20) NULL,
  IsPurchaseOrderMandatory bit NOT NULL,PurchaseOrderExpiryDateBA smalldatetime NULL,ArkadinBankAccountDetailsId varchar(20) NOT NULL,
  PrintHouse nvarchar(255) NOT NULL,TaxExemptID varchar(255) NULL,TaxVendor nvarchar(255) NULL,MetraTaxCountryEligibility nvarchar(255) NULL,
  MetraTaxCountryZone nvarchar(255) NULL,MetraTaxHasOverrideBand bit NULL,MetraTaxOverrideBand nvarchar(255) NULL,
  TaxServiceAddressPCode int NULL,TaxExemptReason nvarchar(255) NULL,TaxExemptionStartDate smalldatetime NULL,TaxExemptionEndDate smalldatetime NULL,
  TaxRegistryReference varchar(255) NULL,BilledThroughDate smalldatetime NOT NULL,EffectiveMgmtSubsidiary nvarchar(255) NULL,
  LogoUserName varchar(255) NOT NULL, IsMigrated char(1));

CREATE TABLE mcTesting.GroupAcct (LegacyAccountId varchar(255) NOT NULL,LegacySystem varchar(50) NOT NULL,ParentLegacyAccountId varchar(255) NULL,
  PayerLegacyAccountId varchar(255) NULL,StartDate smalldatetime NOT NULL,EndDate smalldatetime NULL,AccountStatus varchar(2) NOT NULL,
  AccountPassword varchar(50) NOT NULL,
  CreatedBy varchar(50) NOT NULL,CreatedDate smalldatetime NOT NULL,ModifiedBy varchar(50) NOT NULL,ModifiedDate smalldatetime NOT NULL,
  OrderStatus nvarchar(255) NOT NULL,CustPartnerExtId varchar(50) NULL,CustomerNodeName varchar(255) NOT NULL,CommentC varchar(255) NULL,
  SubManagementPG nvarchar(255) NULL,IsEvent bit NULL,EventPurchaseOrderRef varchar(20) NULL,RqstFlowValidationActivated bit NULL,
  EffectiveMgmtSubsidiary nvarchar(255) NULL,LogoUserName varchar(255) NOT NULL, IsMigrated char(1));

CREATE TABLE mcTesting.PrimaryGroupAcct (LegacyAccountId varchar(255) NOT NULL,LegacySystem varchar(50) NOT NULL,ParentLegacyAccountId varchar(255) NULL,
  PayerLegacyAccountId varchar(255) NULL,StartDate smalldatetime NOT NULL,EndDate smalldatetime NULL,AccountStatus varchar(2) NOT NULL,
  AccountPassword varchar(50) NOT NULL,
  CreatedBy varchar(50) NOT NULL,CreatedDate smalldatetime NOT NULL,ModifiedBy varchar(50) NOT NULL,ModifiedDate smalldatetime NOT NULL,
  OrderStatus nvarchar(255) NOT NULL,CustPartnerExtId varchar(50) NULL,CustomerNodeName varchar(255) NOT NULL,CommentC varchar(255) NULL,
  BranchCountry nvarchar(255) NULL,BranchDepartment varchar(100) NULL,CostCenter varchar(50) NULL,Dispatch1 varchar(50) NULL,Dispatch2 varchar(50) NULL,
  SubManagementPG nvarchar(255) NULL,IsEvent bit NULL,EventPurchaseOrderRef varchar(20) NOT NULL,RqstFlowValidationActivated bit NULL,
  EffectiveMgmtSubsidiary nvarchar(255) NULL,LogoUserName varchar(255) NOT NULL, IsMigrated char(1));

CREATE TABLE mcTesting.UserAcct (LegacyAccountId varchar(255) NOT NULL,LegacySystem varchar(50) NOT NULL,ParentLegacyAccountId varchar(255) NULL,
  PayerLegacyAccountId varchar(255) NULL,StartDate smalldatetime NOT NULL,EndDate smalldatetime NULL,AccountStatus varchar(2) NOT NULL,
  AccountPassword varchar(50) NOT NULL,
  CreatedBy varchar(50) NOT NULL,CreatedDate smalldatetime NOT NULL,ModifiedBy varchar(50) NOT NULL,ModifiedDate smalldatetime NOT NULL,
  OrderStatus nvarchar(255) NOT NULL,CustPartnerExtId varchar(50) NULL,CustomerNodeName varchar(255) NOT NULL,CommentC varchar(255) NULL,
  LoginU varchar(50) NOT NULL,BillingCodeU varchar(255) NULL,SelfcareRelation nvarchar(255) NULL,
  SelfcareRole nvarchar(255) NULL,Moderator bit NOT NULL,SalesForceIDU varchar(15) NULL,
  UserDispatch1 varchar(50) NULL,UserDispatch2 varchar(50) NULL,
  EffectiveMgmtSubsidiary nvarchar(255) NULL,LogoUserName varchar(255) NOT NULL, BillingRefU varchar(50) NULL, IsMigrated char(1));

CREATE TABLE mcTesting.AccessAcct (LegacyAccountId varchar(255) NOT NULL,LegacySystem varchar(50) NOT NULL,ParentLegacyAccountId varchar(255) NULL,
  PayerLegacyAccountId varchar(255) NULL,StartDate smalldatetime NOT NULL,EndDate smalldatetime NULL,AccountStatus varchar(2) NOT NULL,
  CreatedBy varchar(50) NOT NULL,CreatedDate smalldatetime NOT NULL,ModifiedBy varchar(50) NOT NULL,ModifiedDate smalldatetime NOT NULL,
  OrderStatus nvarchar(255) NOT NULL,CustPartnerExtId varchar(50) NULL,CustomerNodeName varchar(255) NOT NULL,CommentC varchar(255) NULL,
  AccessCategory nvarchar(255) NOT NULL,AccessType nvarchar(255) NOT NULL,NetworkElementReference varchar(100) NOT NULL,
  NetworkElementAccessRef varchar(100) NULL,ExternalConfRef varchar(100) NULL,NetworkElementAccessName varchar(100) NULL,
  AudioModeratorPIN varchar(100) NULL,AudioParticipantPIN varchar(100) NULL,AccessLogin varchar(100) NULL,
  AccessPassword varchar(100) NULL,Trial bit NOT NULL,CommentA varchar(255) NULL,
  BillingCode varchar(255) NULL,NetworkElementTechnicalEnv varchar(100) NOT NULL,Topic varchar(255) NULL,
  EffectiveMgmtSubsidiary nvarchar(255) NULL,LogoUserName varchar(255) NOT NULL, IsMigrated char(1));

CREATE TABLE mcTesting.AcctContact (LegacyAccountId varchar(255) NOT NULL,ContactType nvarchar(255) NOT NULL,CommunicationLanguage nvarchar(255) NOT NULL,
  Timezone nvarchar(255) NOT NULL,Salutation varchar(10) NULL,Name1 varchar(100) NULL,Name2 varchar(50) NULL,
  FirstName varchar(40) NULL,MiddleInitial varchar(1) NULL,LastName varchar(100) NOT NULL,Email varchar(255) NOT NULL,
  PhoneNumber varchar(40) NULL,FacsimileTelephoneNumber varchar(40) NULL,Address1 varchar(100) NULL,Address2 varchar(100) NULL,
  Address3 varchar(100) NULL,City varchar(40) NULL,State varchar(40) NULL,Zip varchar(40) NULL,Country nvarchar(255) NULL,
  LocalSalutation varchar(10) NULL,LocalFirstName varchar(40) NULL,LocalMiddleInitial varchar(1) NULL,
  LocalLastName varchar(100) NULL,LocalAddress1 varchar(100) NULL,LocalAddress2 varchar(100) NULL,LocalAddress3 varchar(100) NULL,
  LocalCity varchar(40) NULL,LocalState varchar(255) NULL,Company varchar(100) NULL,
  AccountTypeId int NOT NULL);

CREATE TABLE mcTesting.Names (ID int PRIMARY KEY NOT NULL, Salutation nvarchar(10), FirstName nvarchar(20), MiddleInit nvarchar(1), LastName nvarchar(20));

CREATE TABLE mcTesting.Addresses (ID int PRIMARY KEY NOT NULL, [Address] nvarchar(100), City nvarchar(40), Province nvarchar(40), Country nvarchar(40));


PRINT 'Finished Initialization of variables';

-- ####### CONFIG
-- Specify top-level accounts explicitly
DECLARE @SiloAccounts TABLE (ID int, Label nvarchar(40), DoCreate bit);
DECLARE @PartnerAccounts TABLE (ID int, Label nvarchar(40), DoCreate bit, ParentID int);

-- Configure parameters to form accounts
DECLARE @AcctType TABLE (ID int, Label nvarchar(40), Prefix nvarchar(10));

-- Specify the distribution of accounts for each hierarchy. 
DECLARE @HierarchyTypeAcct TABLE (AcctTypeID int, ParentHierID int, NumAccts int, PRIMARY KEY(AcctTypeID,ParentHierID));

DECLARE @StreetNames TABLE (ID int IDENTITY(1,1), StreetName nvarchar(50));
DECLARE @StreetSuffix TABLE (ID int IDENTITY(1,1), Suffix nvarchar(10));
DECLARE @CityProvince TABLE (ID int IDENTITY(1,1), City nvarchar(40), Province nvarchar(40), Country nvarchar(40));
DECLARE @FirstNames TABLE (ID int IDENTITY(1,1), FirstName nvarchar(20));
DECLARE @MidInits TABLE (ID int IDENTITY(1,1), MI nvarchar(1));
DECLARE @LastNames TABLE (ID int IDENTITY(1,1), LastName nvarchar(20));
DECLARE @Salutations TABLE (ID int IDENTITY(1,1), Salutation nvarchar(10));

-- Value to insure we get uniques across runs
DECLARE @NameSuffix nvarchar(20) = N'_' + CAST(DATEDIFF(ss, '20140101', GETUTCDATE()) AS nvarchar(20));

-- to fill "Created By" and "Modified By"
DECLARE @CreatedBy varchar(50) = 'Tim';
DECLARE @ModifiedBy varchar(50) = 'TCF';

-- Create a table to hold relevant enum configs
DECLARE @EnumFile TABLE (ID int IDENTITY(1,1), URI varchar(255));


PRINT 'Finished Config Setup';

-- ####### CONFIG DATA
INSERT INTO @AcctType (ID, Label, Prefix) VALUES (6,N'SystemAccount',N'Sys');
INSERT INTO @AcctType (ID, Label, Prefix) VALUES (7,N'AccessAccount',N'Acc');
INSERT INTO @AcctType (ID, Label, Prefix) VALUES (8,N'BillingAccount',N'Bill');
INSERT INTO @AcctType (ID, Label, Prefix) VALUES (9,N'GroupAccount',N'Grp');
INSERT INTO @AcctType (ID, Label, Prefix) VALUES (10,N'LegalEntityAccount',N'LE');
INSERT INTO @AcctType (ID, Label, Prefix) VALUES (11,N'LocalLogoAccount',N'LL');
INSERT INTO @AcctType (ID, Label, Prefix) VALUES (12,N'LogoAccount',N'Logo');
INSERT INTO @AcctType (ID, Label, Prefix) VALUES (13,N'PartnerAccount',N'Partner');
INSERT INTO @AcctType (ID, Label, Prefix) VALUES (14,N'PrimaryGroupAccount',N'PGrp');
INSERT INTO @AcctType (ID, Label, Prefix) VALUES (15,N'ReportingGroup1Account',N'RG1');
INSERT INTO @AcctType (ID, Label, Prefix) VALUES (16,N'ReportingGroup2Account',N'RG2');
INSERT INTO @AcctType (ID, Label, Prefix) VALUES (17,N'SiloAccount',N'Silo');
INSERT INTO @AcctType (ID, Label, Prefix) VALUES (18,N'UserAccount',N'User');

INSERT INTO @StreetNames (StreetName) VALUES (N'Main');
INSERT INTO @StreetNames (StreetName) VALUES (N'North');
INSERT INTO @StreetNames (StreetName) VALUES (N'South');
INSERT INTO @StreetNames (StreetName) VALUES (N'East');
INSERT INTO @StreetNames (StreetName) VALUES (N'West');
INSERT INTO @StreetNames (StreetName) VALUES (N'Pine');
INSERT INTO @StreetNames (StreetName) VALUES (N'Walnut');
INSERT INTO @StreetNames (StreetName) VALUES (N'Oak');
INSERT INTO @StreetNames (StreetName) VALUES (N'Birch');
INSERT INTO @StreetNames (StreetName) VALUES (N'Washington');
INSERT INTO @StreetNames (StreetName) VALUES (N'Summer');
INSERT INTO @StreetNames (StreetName) VALUES (N'Spring');
INSERT INTO @StreetNames (StreetName) VALUES (N'Winter');
INSERT INTO @StreetNames (StreetName) VALUES (N'Autumn');
INSERT INTO @StreetNames (StreetName) VALUES (N'State');
INSERT INTO @StreetNames (StreetName) VALUES (N'Forest');
INSERT INTO @StreetNames (StreetName) VALUES (N'Ocean');
INSERT INTO @StreetNames (StreetName) VALUES (N'Boardwalk');
INSERT INTO @StreetNames (StreetName) VALUES (N'Border');
INSERT INTO @StreetNames (StreetName) VALUES (N'Appian');

INSERT INTO @StreetSuffix (Suffix) VALUES (N'Street');
INSERT INTO @StreetSuffix (Suffix) VALUES (N'Avenue');
INSERT INTO @StreetSuffix (Suffix) VALUES (N'Road');
INSERT INTO @StreetSuffix (Suffix) VALUES (N'Lane');
INSERT INTO @StreetSuffix (Suffix) VALUES (N'St');
INSERT INTO @StreetSuffix (Suffix) VALUES (N'Path');
INSERT INTO @StreetSuffix (Suffix) VALUES (N'Ave');
INSERT INTO @StreetSuffix (Suffix) VALUES (N'Rd');
INSERT INTO @StreetSuffix (Suffix) VALUES (N'Ln');
INSERT INTO @StreetSuffix (Suffix) VALUES (N'Way');
INSERT INTO @StreetSuffix (Suffix) VALUES (N'Blvd');

INSERT INTO @CityProvince (City,Province,Country) VALUES (N'Chicago',N'IL',N'USA');
INSERT INTO @CityProvince (City,Province,Country) VALUES (N'Philadelphia',N'PA',N'USA');
INSERT INTO @CityProvince (City,Province,Country) VALUES (N'San Francisco',N'CA',N'USA');
INSERT INTO @CityProvince (City,Province,Country) VALUES (N'Atlanta',N'GA',N'USA');
INSERT INTO @CityProvince (City,Province,Country) VALUES (N'Toronto',N'ON',N'Canada');
INSERT INTO @CityProvince (City,Province,Country) VALUES (N'Paris',NULL,N'France');
INSERT INTO @CityProvince (City,Province,Country) VALUES (N'London',NULL,N'United Kingdom');
INSERT INTO @CityProvince (City,Province,Country) VALUES (N'Lyon',NULL,N'France');
INSERT INTO @CityProvince (City,Province,Country) VALUES (N'Bern',NULL,N'Switzerland');
INSERT INTO @CityProvince (City,Province,Country) VALUES (N'Beijing',NULL,N'China');

--DECLARE @FirstNames TABLE (ID int IDENTITY(1,1), FirstName nvarchar(20));
--DECLARE @MidInits TABLE (ID int IDENTITY(1,1), MI nvarchar(1));
--DECLARE @LastNames TABLE (ID int IDENTITY(1,1), LastName nvarchar(20));

INSERT INTO @FirstNames (FirstName) VALUES (N'Ira');
INSERT INTO @FirstNames (FirstName) VALUES (N'Nan');
INSERT INTO @FirstNames (FirstName) VALUES (N'Pat');
INSERT INTO @FirstNames (FirstName) VALUES (N'El');
INSERT INTO @FirstNames (FirstName) VALUES (N'Jan');
INSERT INTO @FirstNames (FirstName) VALUES (N'Alex');
INSERT INTO @FirstNames (FirstName) VALUES (N'Andy');
INSERT INTO @FirstNames (FirstName) VALUES (N'Chris');
INSERT INTO @FirstNames (FirstName) VALUES (N'Fran');
INSERT INTO @FirstNames (FirstName) VALUES (N'Jerry');
INSERT INTO @FirstNames (FirstName) VALUES (N'Lou');
INSERT INTO @FirstNames (FirstName) VALUES (N'Max');
INSERT INTO @FirstNames (FirstName) VALUES (N'Mel');
INSERT INTO @FirstNames (FirstName) VALUES (N'Nat');
INSERT INTO @FirstNames (FirstName) VALUES (N'Ronny');
INSERT INTO @FirstNames (FirstName) VALUES (N'Sam');
INSERT INTO @FirstNames (FirstName) VALUES (N'Terry');
INSERT INTO @FirstNames (FirstName) VALUES (N'Pascal');
INSERT INTO @FirstNames (FirstName) VALUES (N'Dominique');
INSERT INTO @FirstNames (FirstName) VALUES (N'Ryan');

INSERT INTO @MidInits (MI) VALUES (N'A');
INSERT INTO @MidInits (MI) VALUES (N'B');
INSERT INTO @MidInits (MI) VALUES (N'C');
INSERT INTO @MidInits (MI) VALUES (N'D');
INSERT INTO @MidInits (MI) VALUES (N'E');
INSERT INTO @MidInits (MI) VALUES (N'F');
INSERT INTO @MidInits (MI) VALUES (N'G');
INSERT INTO @MidInits (MI) VALUES (N'H');
INSERT INTO @MidInits (MI) VALUES (N'I');
INSERT INTO @MidInits (MI) VALUES (N'J');
INSERT INTO @MidInits (MI) VALUES (N'K');
INSERT INTO @MidInits (MI) VALUES (N'L');
INSERT INTO @MidInits (MI) VALUES (N'M');
INSERT INTO @MidInits (MI) VALUES (N'N');
INSERT INTO @MidInits (MI) VALUES (N'O');
INSERT INTO @MidInits (MI) VALUES (N'P');
INSERT INTO @MidInits (MI) VALUES (N'Q');
INSERT INTO @MidInits (MI) VALUES (N'R');
INSERT INTO @MidInits (MI) VALUES (N'S');
INSERT INTO @MidInits (MI) VALUES (N'T');
INSERT INTO @MidInits (MI) VALUES (N'U');
INSERT INTO @MidInits (MI) VALUES (N'V');
INSERT INTO @MidInits (MI) VALUES (N'W');
INSERT INTO @MidInits (MI) VALUES (N'X');
INSERT INTO @MidInits (MI) VALUES (N'Y');
INSERT INTO @MidInits (MI) VALUES (N'Z');

INSERT INTO @LastNames (LastName) VALUES (N'Smith');
INSERT INTO @LastNames (LastName) VALUES (N'Cox');
INSERT INTO @LastNames (LastName) VALUES (N'Rogers');
INSERT INTO @LastNames (LastName) VALUES (N'Peterson');
INSERT INTO @LastNames (LastName) VALUES (N'Davis');
INSERT INTO @LastNames (LastName) VALUES (N'Abbott');
INSERT INTO @LastNames (LastName) VALUES (N'Benson');
INSERT INTO @LastNames (LastName) VALUES (N'Carter');
INSERT INTO @LastNames (LastName) VALUES (N'Eldridge');
INSERT INTO @LastNames (LastName) VALUES (N'Fuller');
INSERT INTO @LastNames (LastName) VALUES (N'Garcia');
INSERT INTO @LastNames (LastName) VALUES (N'Henderson');
INSERT INTO @LastNames (LastName) VALUES (N'Jackson');
INSERT INTO @LastNames (LastName) VALUES (N'Keller');
INSERT INTO @LastNames (LastName) VALUES (N'Lewis');
INSERT INTO @LastNames (LastName) VALUES (N'Masterson');
INSERT INTO @LastNames (LastName) VALUES (N'Nichols');
INSERT INTO @LastNames (LastName) VALUES (N'Orbeson');
INSERT INTO @LastNames (LastName) VALUES (N'Porter');
INSERT INTO @LastNames (LastName) VALUES (N'Quenton');
INSERT INTO @LastNames (LastName) VALUES (N'Richards');
INSERT INTO @LastNames (LastName) VALUES (N'Stephens');
INSERT INTO @LastNames (LastName) VALUES (N'Tully');
INSERT INTO @LastNames (LastName) VALUES (N'Martell');
INSERT INTO @LastNames (LastName) VALUES (N'Stark');
INSERT INTO @LastNames (LastName) VALUES (N'Urban');
INSERT INTO @LastNames (LastName) VALUES (N'Williams');

INSERT INTO @Salutations (Salutation) VALUES (N'Mr.');
INSERT INTO @Salutations (Salutation) VALUES (N'Mr.');
INSERT INTO @Salutations (Salutation) VALUES (N'Mr.');
INSERT INTO @Salutations (Salutation) VALUES (N'Mr.');
INSERT INTO @Salutations (Salutation) VALUES (N'Mr.');
INSERT INTO @Salutations (Salutation) VALUES (N'Mr.');
INSERT INTO @Salutations (Salutation) VALUES (N'Mr.');
INSERT INTO @Salutations (Salutation) VALUES (N'Mr.');
INSERT INTO @Salutations (Salutation) VALUES (N'Mr.');
INSERT INTO @Salutations (Salutation) VALUES (N'Mr.');
INSERT INTO @Salutations (Salutation) VALUES (N'Dr.');
INSERT INTO @Salutations (Salutation) VALUES (N'Mrs.');
INSERT INTO @Salutations (Salutation) VALUES (N'Mrs.');
INSERT INTO @Salutations (Salutation) VALUES (N'Mrs.');
INSERT INTO @Salutations (Salutation) VALUES (N'Mrs.');
INSERT INTO @Salutations (Salutation) VALUES (N'Ms.');
INSERT INTO @Salutations (Salutation) VALUES (N'Ms.');
INSERT INTO @Salutations (Salutation) VALUES (N'Ms.');
INSERT INTO @Salutations (Salutation) VALUES (N'Ms.');


-- Enums; need to make sure that enums are valid so pull from meta rather than DB
INSERT INTO @EnumFile (URI) VALUES ('D:\MetraTech\RMP\Extensions\Account\config\enumtype\metratech.com\metratech.com_accountcreation.xml');
INSERT INTO @EnumFile (URI) VALUES ('D:\MetraTech\RMP\Extensions\Arkadin\config\enumtype\metratech.com\metratech.com_EnumAccount.xml');
INSERT INTO @EnumFile (URI) VALUES ('D:\MetraTech\RMP\Extensions\Core\config\enumtype\Global\Global.xml');
INSERT INTO @EnumFile (URI) VALUES ('D:\MetraTech\RMP\Extensions\Core\config\enumtype\metratech.com\metratech.com_billingcycle.xml');
INSERT INTO @EnumFile (URI) VALUES ('D:\MetraTech\RMP\Extensions\Arkadin\config\enumtype\metratech.com\metratech.com_EnumGlobal.xml');
INSERT INTO @EnumFile (URI) VALUES ('D:\MetraTech\RMP\Extensions\Arkadin\config\enumtype\metratech.com\metratech.com_EnumBilling.xml');
INSERT INTO @EnumFile (URI) VALUES ('D:\MetraTech\RMP\Extensions\Arkadin\config\enumtype\metratech.com\metratech.com_EnumPartner.xml');
INSERT INTO @EnumFile (URI) VALUES ('D:\MetraTech\RMP\Extensions\Tax\config\enumtype\metratech.com\metratech.com_tax.xml');
/*INSERT INTO @EnumFile (URI) VALUES ('');
INSERT INTO @EnumFile (URI) VALUES ('');
INSERT INTO @EnumFile (URI) VALUES ('');
INSERT INTO @EnumFile (URI) VALUES ('');*/


PRINT 'Finished inserting config data';

-- ####### RUN SETTINGS
DECLARE @NumSysAccts int = 1;

-- Need to create Silo and Partner accounts even if you don't want to create new ones; if you don't want to 
-- create them, just specify the existing account with DoCreate = 0
-- mimic arkadin, where Silo already in the db
--INSERT INTO @SiloAccounts (ID, Label, DoCreate) VALUES (1, N'Silo' + @NameSuffix, 1);
INSERT INTO @SiloAccounts (ID, Label, DoCreate) VALUES (1, N'Silo-FR', 0);

INSERT INTO @PartnerAccounts (ID, Label, DoCreate, ParentID) VALUES (1, N'PartnerFoo' + @NameSuffix, 1, 1);
INSERT INTO @PartnerAccounts (ID, Label, DoCreate, ParentID) VALUES (2, N'PartnerBar' + @NameSuffix, 1, 1);

-- Configure the different hierarchies. Remember that Silo and Partner are hard-coded; need to set up
-- each account below those, one for each "tree type" that we want it to be a member of.
-- If an account can only have one parent, it should only be listed once here.
/*
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (12,13,50); -- Logo
-- Logo can have Local Logo, Billing, or Legal Entity children
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (11,12,25); -- Local Logo
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (10,12,30); -- Legal Entity
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,12,11);  -- Billing
-- Local Logo can have Billing and Reporting Group 1
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,11,11);  -- Billing
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (15,11,22); -- RG1
-- Reporting Group 1 can have RG2 or Billing
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (16,15,18); -- RG2
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,15,5); -- Billing
-- Reporting Group 2 can have Billing
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,16,6);  -- Billing
-- Billing can have Group and Primary Group
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (9,8,11);   -- Group
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (14,8,10);  -- PG
-- Group can have Primary Group
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (14,9,22);  -- PG
-- Primary Group can have User
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (18,14,120); -- User
-- User can have Access
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (7,18,400); -- Access
*/

/*
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (12,13,1); -- Logo
-- Logo can have Local Logo, Billing, or Legal Entity children
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (11,12,1); -- Local Logo
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (10,12,1); -- Legal Entity
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,12,1);  -- Billing
-- Local Logo can have Billing and Reporting Group 1
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,11,1);  -- Billing
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (15,11,1); -- RG1
-- Reporting Group 1 can have RG2 or Billing
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (16,15,1); -- RG2
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,15,1); -- Billing
-- Reporting Group 2 can have Billing
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,16,1);  -- Billing
-- Billing can have Group and Primary Group
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (9,8,1);   -- Group
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (14,8,1);  -- PG
-- Group can have Primary Group
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (14,9,1);  -- PG
-- Primary Group can have User
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (18,14,5); -- User
-- User can have Access
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (7,18,2000); -- Access
*/

-- About 1000 accounts
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (12,13,2); -- Logo
-- Logo can have Local Logo, Billing, or Legal Entity children
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (11,12,2); -- Local Logo
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (10,12,14); -- Legal Entity
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,12,4);  -- Billing
-- Local Logo can have Billing and Reporting Group 1
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,11,5);  -- Billing
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (15,11,1); -- RG1
-- Reporting Group 1 can have RG2 or Billing
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (16,15,1); -- RG2
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,15,3); -- Billing
-- Reporting Group 2 can have Billing
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,16,3);  -- Billing
-- Billing can have Group and Primary Group
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (9,8,1);   -- Group
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (14,8,20);  -- PG
-- Group can have Primary Group
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (14,9,20);  -- PG
-- Primary Group can have User
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (18,14,273); -- User
-- User can have Access
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (7,18,668); -- Access



-- Mimic the hierarchy from the arkadin test set, 102083 accounts
/*
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (12,13,651); -- Logo
-- Logo can have Local Logo, Billing, or Legal Entity children
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (11,12,70); -- Local Logo
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (10,12,1457); -- Legal Entity
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,12,480);  -- Billing
-- Local Logo can have Billing and Reporting Group 1
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,11,480);  -- Billing
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (15,11,50); -- RG1
-- Reporting Group 1 can have RG2 or Billing
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (16,15,44); -- RG2
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,15,248); -- Billing
-- Reporting Group 2 can have Billing
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,16,249);  -- Billing
-- Billing can have Group and Primary Group
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (9,8,23);   -- Group
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (14,8,2056);  -- PG
-- Group can have Primary Group
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (14,9,2056);  -- PG
-- Primary Group can have User
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (18,14,27367); -- User
-- User can have Access
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (7,18,66851); -- Access
*/

-- About 200,000 accounts
/*
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (12,13,1300); -- Logo
-- Logo can have Local Logo, Billing, or Legal Entity children
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (11,12,140); -- Local Logo
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (10,12,2900); -- Legal Entity
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,12,960);  -- Billing
-- Local Logo can have Billing and Reporting Group 1
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,11,960);  -- Billing
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (15,11,100); -- RG1
-- Reporting Group 1 can have RG2 or Billing
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (16,15,88); -- RG2
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,15,500); -- Billing
-- Reporting Group 2 can have Billing
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,16,500);  -- Billing
-- Billing can have Group and Primary Group
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (9,8,46);   -- Group
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (14,8,4100);  -- PG
-- Group can have Primary Group
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (14,9,4100);  -- PG
-- Primary Group can have User
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (18,14,55000); -- User
-- User can have Access
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (7,18,133000); -- Access
*/

-- even distribution of accounts
/*INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (12,13,10208); -- Logo
-- Logo can have Local Logo, Billing, or Legal Entity children
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (11,12,10208); -- Local Logo
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (10,12,10208); -- Legal Entity
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,12,2552);  -- Billing
-- Local Logo can have Billing and Reporting Group 1
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,11,2552);  -- Billing
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (15,11,10208); -- RG1
-- Reporting Group 1 can have RG2 or Billing
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (16,15,10208); -- RG2
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,15,2552); -- Billing
-- Reporting Group 2 can have Billing
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,16,2552);  -- Billing
-- Billing can have Group and Primary Group
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (9,8,10208);   -- Group
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (14,8,5104);  -- PG
-- Group can have Primary Group
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (14,9,5104);  -- PG
-- Primary Group can have User
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (18,14,10208); -- User
-- User can have Access
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (7,18,10208); -- Access*/

-- Do a hierarchy with only 6 levels populated
/*INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (12,13,83); -- Logo
-- Logo can have Local Logo, Billing, or Legal Entity children
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,12,1994);  -- Billing
-- Billing can have Group and Primary Group
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (14,8,15970);  -- PG
-- Primary Group can have User
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (18,14,17183); -- User
-- User can have Access
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (7,18,66851); -- Access*/

-- 1 million+ acccounts
/*
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (12,13,6510); -- Logo
-- Logo can have Local Logo, Billing, or Legal Entity children
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (11,12,700); -- Local Logo
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (10,12,14570); -- Legal Entity
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,12,4800);  -- Billing
-- Local Logo can have Billing and Reporting Group 1
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,11,4800);  -- Billing
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (15,11,500); -- RG1
-- Reporting Group 1 can have RG2 or Billing
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (16,15,440); -- RG2
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,15,2480); -- Billing
-- Reporting Group 2 can have Billing
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (8,16,2490);  -- Billing
-- Billing can have Group and Primary Group
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (9,8,230);   -- Group
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (14,8,20560);  -- PG
-- Group can have Primary Group
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (14,9,20560);  -- PG
-- Primary Group can have User
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (18,14,273670); -- User
-- User can have Access
INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (7,18,668510); -- Access
*/

/*
1208	ReportingGroup1Account
1161	GroupAccount
2687	LogoAccount
26712	AccessAccount
11298	UserAccount
2979	LegalEntityAccount
30	PartnerAccount
1115	ReportingGroup2Account
1	SiloAccount
1378	LocalLogoAccount
3194	PrimaryGroupAccount
3216	BillingAccount
*/

-- ####### POPULATE TEMP TABLES WITH OUR ACCOUNTS
DECLARE @i int;

-- Loop through all of the applicable enum XML; dump it into a table to use here
DECLARE @MyURI varchar(255);
DECLARE @xml xml;
DECLARE @handle int;
DECLARE @sql nvarchar(2000);
DECLARE @f bit;
DECLARE @g bit;
DECLARE E CURSOR FAST_FORWARD FOR
  SELECT URI
  FROM @EnumFile
  ORDER BY ID;
OPEN E
FETCH NEXT FROM E INTO @MyURI;
WHILE @@fetch_status = 0
  BEGIN
    SET @sql = 'SELECT @xml = CAST(x AS XML) FROM OPENROWSET(BULK ''' + @MyURI + ''', SINGLE_BLOB) AS T(x)';
    EXECUTE sp_executesql @sql, N'@MyURI varchar(255), @xml xml OUTPUT', @MyURI, @xml=@xml OUTPUT;

    -- Yes we have different names for root nodes OMG...
    SET @f = @xml.exist('/mt_enum_config');
    SET @g = @xml.exist('/mt_config');

    IF @f = 1
    BEGIN
      INSERT INTO mcTesting.Enums (EnumFull, Enum, Val, EnumNS, Rnk)
        SELECT cte.EnumGrp + N'/' + cte.Val AS EnumFull, cte.Enum, cte.Val, cte.NmSpc,
        ROW_NUMBER() OVER(PARTITION BY cte.EnumGrp ORDER BY cte.Val) AS Rnk
        FROM (SELECT vals.value('../../../../@name','nvarchar(255)') + N'/' + vals.value('../../@name','nvarchar(255)') AS EnumGrp,
        vals.value('../../@name','nvarchar(255)') AS Enum, vals.value('@name','nvarchar(255)') AS Val,
        vals.value('../../../../@name','nvarchar(255)') AS NmSpc
        FROM @xml.nodes('/mt_enum_config/enum_spaces/enum_space/enums/enum/entries/entry') X(vals)) AS cte
        INNER JOIN dbo.t_enum_data AS E ON (E.nm_enum_data = (cte.EnumGrp + N'/' + cte.Val))
    END;

    IF @g = 1
    BEGIN
      INSERT INTO mcTesting.Enums (EnumFull, Enum, Val, EnumNS, Rnk)
        SELECT cte.EnumGrp + N'/' + cte.Val AS EnumFull, cte.Enum, cte.Val, cte.NmSpc,
        ROW_NUMBER() OVER(PARTITION BY cte.EnumGrp ORDER BY cte.Val) AS Rnk
        FROM (SELECT vals.value('../../../../@name','nvarchar(255)') + N'/' + vals.value('../../@name','nvarchar(255)') AS EnumGrp,
        vals.value('../../@name','nvarchar(255)') AS Enum, vals.value('@name','nvarchar(255)') AS Val,
        vals.value('../../../../@name','nvarchar(255)') AS NmSpc
        FROM @xml.nodes('/mt_config/enum_spaces/enum_space/enums/enum/entries/entry') X(vals)) AS cte
        INNER JOIN dbo.t_enum_data AS E ON (E.nm_enum_data = (cte.EnumGrp + N'/' + cte.Val))
    END;
    
    FETCH NEXT FROM E INTO @MyURI;
  END
CLOSE E;
DEALLOCATE E;

--select * from mcTesting.Enums where Val LIKE '%Ivo%'
-- WORKAROUND; THIS ENUM KEEPS GETTING CORRUPTED IN THE CONVERSION TO UTF-8
DELETE FROM mcTesting.Enums WHERE Val = 'Côte D''Ivoire' AND Enum = 'CountryName';
DELETE FROM mcTesting.Enums WHERE Val = 'Arkadin Côte D''Ivoire' AND Enum = 'SubsidiaryName';


-- Now write Enum header records
INSERT INTO mcTesting.EnumHeader (EnumNS, Enum, Cnt)
  SELECT EnumNS, Enum, COUNT(*)
  FROM mcTesting.Enums
  GROUP BY EnumNS, Enum;

PRINT 'Finished enum initialization';

-- Silo accounts
INSERT INTO mcTesting.SiloAcct (LegacyAccountId, LegacySystem, StartDate, EndDate, AccountStatus, CustomerNodeName, IsMigrated)
  SELECT Label, N'Not Applicable', '20050101', NULL, 'AC', 'sfs89fwfwiof', '1'
  FROM @SiloAccounts
  WHERE DoCreate = 1;
PRINT 'Inserted to #SiloAcct';

-- Insert to AcctName
INSERT INTO mcTesting.AcctName (ID, LegacyAccountId, AccountTypeID)
  SELECT ROW_NUMBER() OVER(ORDER BY LegacyAccountId), LegacyAccountId, 17
  FROM mcTesting.SiloAcct;

-- Partner accounts
INSERT INTO mcTesting.PartnerAcct (LegacyAccountId, LegacySystem, ParentLegacyAccountId, 
  PayerLegacyAccountId, StartDate , EndDate , AccountStatus, 
  AccountPassword, CreatedBy, CreatedDate , ModifiedBy, 
  ModifiedDate , OrderStatus, CustPartnerExtId, CustomerNodeName, 
  CommentC, [Language], Currency, Timezone, 
  SecurityQuestion, SecurityAnswer, UsageCycleType, 
  PaymentMethod, RegistrationNumber, SalesforceIDB, SubBillingB, 
  SubManagementB, SubRevenueB, NavisionInstance, PaymentTermCode, 
  InvoiceDeliveryMethod, InvoiceOutputFormat, InvoiceLanguage, 
  InvoiceMinimumAmount, LegalEntityInheritance, CentralBillingFlagBA, CUDTemplate, 
  EUDTemplate, IsCoverPageRequired, PurchaseOrderReference, IsPurchaseOrderMandatory, 
  PurchaseOrderExpiryDate, ArkadinBankAccountDetailsId, PrintHouse, PartnerRelationshipType, 
  PartnershipStartDate , PartnershipEndDate , CRMonBehalfOf, ARonBehalfOf, MarketingonBehalfOf, 
  PartnerRegion, TaxExemptID, TaxVendor, MetraTaxCountryEligibility, 
  MetraTaxCountryZone, MetraTaxHasOverrideBand, MetraTaxOverrideBand, TaxServiceAddressPCode, 
  TaxExemptReason, TaxExemptionStartDate , TaxExemptionEndDate , TaxRegistryReference, BilledThroughDate, IsMigrated)
  SELECT P.Label, N'Not Applicable', S.Label,
  NULL, '20050201', NULL, 'AC',
  '123', @CreatedBy, '20050201', @ModifiedBy,
  '20050201', 'Lock1', NULL, CAST(NEWID() AS varchar(50)),
  'generated account', 'US', 'EUR', '(GMT+01:00) Paris, Madrid, Amsterdam',
  'What was the name of your first stuffed animal?', 'Teddy', 'Monthly',
  'Cheque', NULL, 'COMEBACKTOME', 'Arkadin France',
  'Arkadin France', 'Arkadin France', 'Arkadin FR', '90D',
  'Mail', 'PDF', 'US', 
  10, 'None', 0, 'None',
  'None', 1, 'PurchOrdRef', 0,
  NULL, 'A12345', 'Esker', 'Direct',
  '20001225', NULL, 0, 0, 0,
  'PartnerRegion', '98765b', 'MetraTax', 'Arkadin France',
  'Tax', 0, NULL, 1,
  'NotExempt', NULL, NULL, NULL, GETDATE(), '1'
  FROM @PartnerAccounts AS P
  INNER JOIN @SiloAccounts AS S ON S.ID = P.ParentID
  WHERE P.DoCreate = 1;
PRINT 'Inserted to #PartnerAccount';

-- Insert to AcctName
INSERT INTO mcTesting.AcctName (ID, LegacyAccountId, AccountTypeID)
  SELECT ROW_NUMBER() OVER(ORDER BY LegacyAccountId), LegacyAccountId, 13
  FROM mcTesting.PartnerAcct;

-- INSERT INTO @HierarchyTypeAcct (AcctTypeID,ParentHierID,NumAccts) VALUES (12,13,50); -- Logo
-- INSERT INTO @AcctType (ID, Label, Prefix) VALUES (12,N'LogoAccount',N'Logo');
DECLARE @iParentAcc int;
DECLARE @iCount int;
DECLARE @AccumulateAcc int;
DECLARE @iCountParent int;

-- iterate through Logo accounts
SET @AccumulateAcc = 1;
DECLARE ACC CURSOR FAST_FORWARD FOR
  SELECT H.ParentHierID, H.NumAccts
  FROM @HierarchyTypeAcct AS H
  INNER JOIN @AcctType AS A ON A.ID = H.AcctTypeID
  WHERE A.Label = 'LogoAccount';
OPEN ACC
FETCH NEXT FROM ACC INTO @iParentAcc, @iCount;
WHILE @@fetch_status = 0
  BEGIN
    SELECT @iCountParent = COUNT(*) FROM mcTesting.PartnerAcct;

    INSERT INTO mcTesting.LogoAcct (LegacyAccountId,LegacySystem,ParentLegacyAccountId,
    PayerLegacyAccountId,StartDate,EndDate,AccountStatus,
    CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,
    OrderStatus,CustPartnerExtId,CustomerNodeName,CommentC,
    SubManagementL,CentralBillingFlagL,EffectiveMgmtSubsidiary, IsMigrated)
      SELECT ('Logo_' + @NameSuffix + '_' + CAST(c.n AS varchar) + '_' + CAST(@AccumulateAcc AS varchar)), 'Not Applicable', a.LegacyAccountId,
      NULL, '20050301', NULL, 'AC',
      @CreatedBy, '20050301', @ModifiedBy, '20050301',
      enmOrderStatus.Val, ('foo_' + @NameSuffix + '_' + CAST(c.n AS varchar)),CAST(NEWID() AS varchar(50)),CAST(NEWID() AS varchar(50)),
      enmSub.Val, (c.n % 2), enmSub.Val, '1'
      FROM mcTesting.timGetNums(1, @iCount) AS c
      INNER JOIN mcTesting.AcctName AS a ON a.ID = ((c.n % @iCountParent) + 1) AND a.AccountTypeID = @iParentAcc
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'OMLockStatus', 'metratech.com/EnumAccount') AS enmOrderStatus ON enmOrderStatus.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'SubsidiaryName', 'metratech.com/EnumGlobal') AS enmSub ON enmSub.n = c.n

    SET @AccumulateAcc = @AccumulateAcc + @iCount;
    FETCH NEXT FROM ACC INTO @iParentAcc, @iCount;
  END
CLOSE ACC;
DEALLOCATE ACC;

-- Insert to AcctName
INSERT INTO mcTesting.AcctName (ID, LegacyAccountId, AccountTypeID)
  SELECT ROW_NUMBER() OVER(ORDER BY LegacyAccountId), LegacyAccountId, 12
  FROM mcTesting.LogoAcct;


-- iterate through Local Logo accounts
SET @AccumulateAcc = 1;
DECLARE ACC CURSOR FAST_FORWARD FOR
  SELECT H.ParentHierID, H.NumAccts
  FROM @HierarchyTypeAcct AS H
  INNER JOIN @AcctType AS A ON A.ID = H.AcctTypeID
  WHERE A.Label = 'LocalLogoAccount';
OPEN ACC
FETCH NEXT FROM ACC INTO @iParentAcc, @iCount;
WHILE @@fetch_status = 0
  BEGIN
    SELECT @iCountParent = COUNT(*) FROM mcTesting.AcctName WHERE AccountTypeID = @iParentAcc;

    INSERT INTO mcTesting.LocalLogoAcct (LegacyAccountId,LegacySystem,ParentLegacyAccountId,
    PayerLegacyAccountId,StartDate,EndDate,AccountStatus,
    CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,
    OrderStatus,CustPartnerExtId,CustomerNodeName,CommentC,
    SubManagementLL, EffectiveMgmtSubsidiary, LogoUserName, IsMigrated)
      SELECT ('LocLogo_' + @NameSuffix + '_' + CAST(c.n AS varchar) + '_' + CAST(@AccumulateAcc AS varchar)), 'Not Applicable', a.LegacyAccountId,
      NULL, '20050301', NULL, 'AC',
      @CreatedBy, '20050301', @ModifiedBy, '20050301',
      enmOrderStatus.Val, ('foo_' + @NameSuffix + '_' + CAST(c.n AS varchar)),CAST(NEWID() AS varchar(50)),CAST(NEWID() AS varchar(50)),
      enmSub.Val, enmSub.Val, ('uname_' + @NameSuffix + '_' + CAST(c.n AS varchar)), '1'
      FROM mcTesting.timGetNums(1, @iCount) AS c
      INNER JOIN mcTesting.AcctName AS a ON a.ID = ((c.n % @iCountParent) + 1) AND a.AccountTypeID = @iParentAcc
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'OMLockStatus', 'metratech.com/EnumAccount') AS enmOrderStatus ON enmOrderStatus.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'SubsidiaryName', 'metratech.com/EnumGlobal') AS enmSub ON enmSub.n = c.n

    SET @AccumulateAcc = @AccumulateAcc + @iCount;
    FETCH NEXT FROM ACC INTO @iParentAcc, @iCount;
  END
CLOSE ACC;
DEALLOCATE ACC;

-- Insert to AcctName
INSERT INTO mcTesting.AcctName (ID, LegacyAccountId, AccountTypeID)
  SELECT ROW_NUMBER() OVER(ORDER BY LegacyAccountId), LegacyAccountId, 11
  FROM mcTesting.LocalLogoAcct;


-- iterate through Legal Entity accounts
SET @AccumulateAcc = 1;
DECLARE ACC CURSOR FAST_FORWARD FOR
  SELECT H.ParentHierID, H.NumAccts
  FROM @HierarchyTypeAcct AS H
  INNER JOIN @AcctType AS A ON A.ID = H.AcctTypeID
  WHERE A.Label = 'LegalEntityAccount';
OPEN ACC
FETCH NEXT FROM ACC INTO @iParentAcc, @iCount;
WHILE @@fetch_status = 0
  BEGIN
    SELECT @iCountParent = COUNT(*) FROM mcTesting.AcctName WHERE AccountTypeID = @iParentAcc;

    INSERT INTO mcTesting.LegalEntityAcct (LegacyAccountId,LegacySystem,ParentLegacyAccountId,
    PayerLegacyAccountId,StartDate,EndDate,AccountStatus,
    CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,
    OrderStatus,CustPartnerExtId,CustomerNodeName,CommentC, /***/
    SubBilling,PaymentTermLE,CommercialRegistrationNumber,
    TaxRegistrationNumber,BillingCurrency,EffectiveMgmtSubsidiary,
    LogoUserName)
      SELECT ('LegEnt_' + @NameSuffix + '_' + CAST(c.n AS varchar) + '_' + CAST(@AccumulateAcc AS varchar)), 'Not Applicable', a.LegacyAccountId,
      NULL, '20050301', NULL, 'AC',
      @CreatedBy, '20050301', @ModifiedBy, '20050301',
      enmOrderStatus.Val, ('foo_' + @NameSuffix + '_' + CAST(c.n AS varchar)),CAST(NEWID() AS varchar(50)),CAST(NEWID() AS varchar(50)),
      enmSub.Val, enmPayTerm.Val, 'RegNumber', 
      'TaxRegNumber', 'USD', enmSub.Val,
      ('unameL_' + @NameSuffix + '_' + CAST(c.n AS varchar))
      FROM mcTesting.timGetNums(1, @iCount) AS c
      INNER JOIN mcTesting.AcctName AS a ON a.ID = ((c.n % @iCountParent) + 1) AND a.AccountTypeID = @iParentAcc
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'OMLockStatus', 'metratech.com/EnumAccount') AS enmOrderStatus ON enmOrderStatus.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'SubsidiaryName', 'metratech.com/EnumGlobal') AS enmSub ON enmSub.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'PaymentTerm', 'metratech.com/EnumBilling') AS enmPayTerm ON enmPayTerm.n = c.n

    SET @AccumulateAcc = @AccumulateAcc + @iCount;
    FETCH NEXT FROM ACC INTO @iParentAcc, @iCount;
  END
CLOSE ACC;
DEALLOCATE ACC;

-- Insert to AcctName
INSERT INTO mcTesting.AcctName (ID, LegacyAccountId, AccountTypeID)
  SELECT ROW_NUMBER() OVER(ORDER BY LegacyAccountId), LegacyAccountId, 10
  FROM mcTesting.LegalEntityAcct;


-- iterate through RG1 accounts
SET @AccumulateAcc = 1;
DECLARE ACC CURSOR FAST_FORWARD FOR
  SELECT H.ParentHierID, H.NumAccts
  FROM @HierarchyTypeAcct AS H
  INNER JOIN @AcctType AS A ON A.ID = H.AcctTypeID
  WHERE A.Label = 'ReportingGroup1Account';
OPEN ACC
FETCH NEXT FROM ACC INTO @iParentAcc, @iCount;
WHILE @@fetch_status = 0
  BEGIN
    SELECT @iCountParent = COUNT(*) FROM mcTesting.AcctName WHERE AccountTypeID = @iParentAcc;

    INSERT INTO mcTesting.RepGrpOneAcct(LegacyAccountId,LegacySystem,ParentLegacyAccountId,
    PayerLegacyAccountId,StartDate,EndDate,AccountStatus,
    CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,
    OrderStatus,CustPartnerExtId,CustomerNodeName,CommentC, /***/
    EffectiveMgmtSubsidiary,LogoUserName, IsMigrated)
      SELECT ('RG1_' + @NameSuffix + '_' + CAST(c.n AS varchar) + '_' + CAST(@AccumulateAcc AS varchar)), 'Not Applicable', a.LegacyAccountId,
      NULL, '20050301', NULL, 'AC',
      @CreatedBy, '20050301', @ModifiedBy, '20050301',
      enmOrderStatus.Val, ('foo_' + @NameSuffix + '_' + CAST(c.n AS varchar)),CAST(NEWID() AS varchar(50)),CAST(NEWID() AS varchar(50)),
      enmSub.Val, ('unameL_' + @NameSuffix + '_' + CAST(c.n AS varchar)), '1'
      FROM mcTesting.timGetNums(1, @iCount) AS c
      INNER JOIN mcTesting.AcctName AS a ON a.ID = ((c.n % @iCountParent) + 1) AND a.AccountTypeID = @iParentAcc
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'OMLockStatus', 'metratech.com/EnumAccount') AS enmOrderStatus ON enmOrderStatus.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'SubsidiaryName', 'metratech.com/EnumGlobal') AS enmSub ON enmSub.n = c.n
      
    SET @AccumulateAcc = @AccumulateAcc + @iCount;
    FETCH NEXT FROM ACC INTO @iParentAcc, @iCount;
  END
CLOSE ACC;
DEALLOCATE ACC;

-- Insert to AcctName
INSERT INTO mcTesting.AcctName (ID, LegacyAccountId, AccountTypeID)
  SELECT ROW_NUMBER() OVER(ORDER BY LegacyAccountId), LegacyAccountId, 15
  FROM mcTesting.RepGrpOneAcct;


-- iterate through RG2 accounts
SET @AccumulateAcc = 1;
DECLARE ACC CURSOR FAST_FORWARD FOR
  SELECT H.ParentHierID, H.NumAccts
  FROM @HierarchyTypeAcct AS H
  INNER JOIN @AcctType AS A ON A.ID = H.AcctTypeID
  WHERE A.Label = 'ReportingGroup2Account';
OPEN ACC
FETCH NEXT FROM ACC INTO @iParentAcc, @iCount;
WHILE @@fetch_status = 0
  BEGIN
    SELECT @iCountParent = COUNT(*) FROM mcTesting.AcctName WHERE AccountTypeID = @iParentAcc;

    INSERT INTO mcTesting.RepGrpTwoAcct(LegacyAccountId,LegacySystem,ParentLegacyAccountId,
    PayerLegacyAccountId,StartDate,EndDate,AccountStatus,
    CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,
    OrderStatus,CustPartnerExtId,CustomerNodeName,CommentC,
    EffectiveMgmtSubsidiary,LogoUserName, IsMigrated)
      SELECT ('RG2_' + @NameSuffix + '_' + CAST(c.n AS varchar) + '_' + CAST(@AccumulateAcc AS varchar)), 'Not Applicable', a.LegacyAccountId,
      NULL, '20050301', NULL, 'AC',
      @CreatedBy, '20050301', @ModifiedBy, '20050301',
      enmOrderStatus.Val, ('foo_' + @NameSuffix + '_' + CAST(c.n AS varchar)),CAST(NEWID() AS varchar(50)),CAST(NEWID() AS varchar(50)),
      enmSub.Val, ('unameL_' + @NameSuffix + '_' + CAST(c.n AS varchar)), '1'
      FROM mcTesting.timGetNums(1, @iCount) AS c
      INNER JOIN mcTesting.AcctName AS a ON a.ID = ((c.n % @iCountParent) + 1) AND a.AccountTypeID = @iParentAcc
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'OMLockStatus', 'metratech.com/EnumAccount') AS enmOrderStatus ON enmOrderStatus.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'SubsidiaryName', 'metratech.com/EnumGlobal') AS enmSub ON enmSub.n = c.n

    SET @AccumulateAcc = @AccumulateAcc + @iCount;
    FETCH NEXT FROM ACC INTO @iParentAcc, @iCount;
  END
CLOSE ACC;
DEALLOCATE ACC;

-- Insert to AcctName
INSERT INTO mcTesting.AcctName (ID, LegacyAccountId, AccountTypeID)
  SELECT ROW_NUMBER() OVER(ORDER BY LegacyAccountId), LegacyAccountId, 16
  FROM mcTesting.RepGrpTwoAcct;


-- iterate through Billing accounts
SET @AccumulateAcc = 1;
DECLARE ACC CURSOR FAST_FORWARD FOR
  SELECT H.ParentHierID, H.NumAccts
  FROM @HierarchyTypeAcct AS H
  INNER JOIN @AcctType AS A ON A.ID = H.AcctTypeID
  WHERE A.Label = 'BillingAccount';
OPEN ACC
FETCH NEXT FROM ACC INTO @iParentAcc, @iCount;
WHILE @@fetch_status = 0
  BEGIN
    SELECT @iCountParent = COUNT(*) FROM mcTesting.AcctName WHERE AccountTypeID = @iParentAcc;

    INSERT INTO mcTesting.BillingAcct (LegacyAccountId,LegacySystem,ParentLegacyAccountId,
    PayerLegacyAccountId,StartDate,EndDate,AccountStatus,
    AccountPassword,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,
    OrderStatus,CustPartnerExtId,CustomerNodeName,CommentC,
    [Language],Currency,Timezone,SecurityQuestion,
    SecurityAnswer,UsageCycleType,PaymentMethod, RegistrationNumber,
    SalesforceIDB, SubBillingB,SubManagementB,SubRevenueB,
    NavisionInstance,PaymentTermCode,InvoiceDeliveryMethod,
    InvoiceOutputFormat,InvoiceLanguage,InvoiceMinimumAmount,
    MasterLegalEntity,LegalEntityInheritance,CentralBillingFlagBA,
    CUDTemplate,EUDTemplate,IsCoverPageRequired,PurchaseOrderReference,
    IsPurchaseOrderMandatory,PurchaseOrderExpiryDateBA,ArkadinBankAccountDetailsId,
    PrintHouse,TaxExemptID,
    TaxVendor,
    MetraTaxCountryEligibility,
    MetraTaxCountryZone,
    MetraTaxHasOverrideBand,
    MetraTaxOverrideBand,
    TaxServiceAddressPCode,TaxExemptReason,TaxExemptionStartDate,TaxExemptionEndDate,
    TaxRegistryReference,BilledThroughDate,EffectiveMgmtSubsidiary,
    LogoUserName, IsMigrated)
      SELECT ('Bill_' + @NameSuffix + '_' + CAST(c.n AS varchar) + '_' + CAST(@AccumulateAcc AS varchar)), 'Not Applicable', a.LegacyAccountId,
      NULL, '20050301', NULL, 'AC',
      CAST(NEWID() AS varchar(50)),@CreatedBy, '20050301', @ModifiedBy, '20050301',
      enmOrderStatus.Val, ('foo_' + @NameSuffix + '_' + CAST(c.n AS varchar)),CAST(NEWID() AS varchar(50)),CAST(NEWID() AS varchar(50)),
      enmLang.Val,'USD',enmTZ.Val,enmSecQ.Val,
      '123', 'Monthly', enmPayMet.Val, '12345',
      ('sf' + @NameSuffix + CAST(c.n AS varchar)), enmSub.Val, enmSub.Val, enmSub.Val,
      enmNavis.Val, enmPayTerm.Val, enmInvDvy.Val,
      enmInvOutFor.Val, enmLang.Val, 100.0,
      NULL, enmLegEntInh.Val, (c.n % 2),
      enmCUD.Val, enmEUD.Val, (c.n % 2), ('po' + @NameSuffix + CAST(c.n AS varchar)),
      ((c.n + 1) % 2), '20171225', ('ba' + @NameSuffix + CAST(c.n AS varchar)),
      'Esker',CAST(NEWID() AS varchar(50)), 
      CASE WHEN enmSub.Val IN ('Arkadin Canada','Arkadin US') THEN 'BillSoft' ELSE 'MetraTax' END, 
      enmSub.Val,
      CASE WHEN enmSub.Val IN ('Arkadin Canada','Arkadin US') THEN NULL ELSE enmTaxZone.Val END,
      CASE WHEN enmSub.Val IN ('Arkadin Canada','Arkadin US') THEN NULL ELSE ((c.n + 1) % 2) END,
      CASE WHEN enmSub.Val IN ('Arkadin Canada','Arkadin US') OR (((c.n + 1) % 2) = 0) THEN NULL ELSE enmTaxBand.Val END,
      c.n, enmTaxExmp.Val, '20050302', NULL,
      CAST(NEWID() AS varchar(50)), '20131231', enmSub.Val,
      ('LogoForBill_' + @NameSuffix + '_' + CAST(c.n AS varchar)), '1'
      FROM mcTesting.timGetNums(1, @iCount) AS c
      INNER JOIN mcTesting.AcctName AS a ON a.ID = ((c.n % @iCountParent) + 1) AND a.AccountTypeID = @iParentAcc
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'OMLockStatus', 'metratech.com/EnumAccount') AS enmOrderStatus ON enmOrderStatus.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'SubsidiaryName', 'metratech.com/EnumGlobal') AS enmSub ON enmSub.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'LanguageCode', 'Global') AS enmLang ON enmLang.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'TimeZoneID', 'Global') AS enmTZ ON enmTZ.n = c.n 
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'SecurityQuestion', 'metratech.com/accountcreation') AS enmSecQ ON enmSecQ.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'PaymentMethod', 'metratech.com/accountcreation') AS enmPayMet ON enmPayMet.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'NavisionInstance', 'metratech.com/EnumGlobal') AS enmNavis ON enmNavis.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'PaymentTerm', 'metratech.com/EnumBilling') AS enmPayTerm ON enmPayTerm.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'InvoiceDeliveryMethod', 'metratech.com/EnumBilling') AS enmInvDvy ON enmInvDvy.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'InvoiceOutputFormat', 'metratech.com/EnumBilling') AS enmInvOutFor ON enmInvOutFor.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'LegalEntityInheritance', 'metratech.com/EnumAccount') AS enmLegEntInh ON enmLegEntInh.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'CUDTemplate', 'metratech.com/EnumBilling') AS enmCUD ON enmCUD.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'EUDTemplate', 'metratech.com/EnumBilling') AS enmEUD ON enmEUD.n = c.n
      --INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'TaxVendor', 'metratech.com/tax') AS enmTax ON enmTax.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'TaxZone', 'metratech.com/tax') AS enmTaxZone ON enmTaxZone.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'TaxBand', 'metratech.com/tax') AS enmTaxBand ON enmTaxBand.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'TaxExemptReason', 'metratech.com/tax') AS enmTaxExmp ON enmTaxExmp.n = c.n


    SET @AccumulateAcc = @AccumulateAcc + @iCount;
    FETCH NEXT FROM ACC INTO @iParentAcc, @iCount;
  END
CLOSE ACC;
DEALLOCATE ACC;

-- Insert to AcctName
INSERT INTO mcTesting.AcctName (ID, LegacyAccountId, AccountTypeID)
  SELECT ROW_NUMBER() OVER(ORDER BY LegacyAccountId), LegacyAccountId, 8
  FROM mcTesting.BillingAcct;


-- iterate through Group accounts
SET @AccumulateAcc = 1;
DECLARE ACC CURSOR FAST_FORWARD FOR
  SELECT H.ParentHierID, H.NumAccts
  FROM @HierarchyTypeAcct AS H
  INNER JOIN @AcctType AS A ON A.ID = H.AcctTypeID
  WHERE A.Label = 'GroupAccount';
OPEN ACC
FETCH NEXT FROM ACC INTO @iParentAcc, @iCount;
WHILE @@fetch_status = 0
  BEGIN
    SELECT @iCountParent = COUNT(*) FROM mcTesting.AcctName WHERE AccountTypeID = @iParentAcc;

    INSERT INTO mcTesting.GroupAcct(LegacyAccountId,LegacySystem,ParentLegacyAccountId,
    PayerLegacyAccountId,StartDate,EndDate,AccountStatus,
    AccountPassword,
    CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,
    OrderStatus,CustPartnerExtId,CustomerNodeName,CommentC,
    SubManagementPG,IsEvent,EventPurchaseOrderRef,RqstFlowValidationActivated,
    EffectiveMgmtSubsidiary,LogoUserName, IsMigrated)
      SELECT ('Grp_' + @NameSuffix + '_' + CAST(c.n AS varchar) + '_' + CAST(@AccumulateAcc AS varchar)), 'Not Applicable', a.LegacyAccountId,
      NULL, '20050301', NULL, 'AC',
      '123',
      @CreatedBy, '20050301', @ModifiedBy, '20050301',
      enmOrderStatus.Val, ('foo_' + @NameSuffix + '_' + CAST(c.n AS varchar)),CAST(NEWID() AS varchar(50)),CAST(NEWID() AS varchar(50)),
      enmSub.Val, (c.n %2), ('po' + @NameSuffix + CAST(c.n AS varchar)), (c.n %2),
      enmSub.Val, ('unameL_' + @NameSuffix + '_' + CAST(c.n AS varchar)), '1'
      FROM mcTesting.timGetNums(1, @iCount) AS c
      INNER JOIN mcTesting.AcctName AS a ON a.ID = ((c.n % @iCountParent) + 1) AND a.AccountTypeID = @iParentAcc
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'OMLockStatus', 'metratech.com/EnumAccount') AS enmOrderStatus ON enmOrderStatus.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'SubsidiaryName', 'metratech.com/EnumGlobal') AS enmSub ON enmSub.n = c.n

    SET @AccumulateAcc = @AccumulateAcc + @iCount;
    FETCH NEXT FROM ACC INTO @iParentAcc, @iCount;
  END
CLOSE ACC;
DEALLOCATE ACC;

-- Insert to AcctName
INSERT INTO mcTesting.AcctName (ID, LegacyAccountId, AccountTypeID)
  SELECT ROW_NUMBER() OVER(ORDER BY LegacyAccountId), LegacyAccountId, 9
  FROM mcTesting.GroupAcct;


-- iterate through PG accounts
SET @AccumulateAcc = 1;
DECLARE ACC CURSOR FAST_FORWARD FOR
  SELECT H.ParentHierID, H.NumAccts
  FROM @HierarchyTypeAcct AS H
  INNER JOIN @AcctType AS A ON A.ID = H.AcctTypeID
  WHERE A.Label = 'PrimaryGroupAccount';
OPEN ACC
FETCH NEXT FROM ACC INTO @iParentAcc, @iCount;
WHILE @@fetch_status = 0
  BEGIN
    SELECT @iCountParent = COUNT(*) FROM mcTesting.AcctName WHERE AccountTypeID = @iParentAcc;

    INSERT INTO mcTesting.PrimaryGroupAcct(LegacyAccountId,LegacySystem,ParentLegacyAccountId,
    PayerLegacyAccountId,StartDate,EndDate,AccountStatus,
    AccountPassword,
    CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,
    OrderStatus,CustPartnerExtId,CustomerNodeName,CommentC,
    BranchCountry,BranchDepartment,CostCenter,Dispatch1,Dispatch2,
    SubManagementPG,IsEvent,EventPurchaseOrderRef,RqstFlowValidationActivated,
    EffectiveMgmtSubsidiary,LogoUserName, IsMigrated)
      SELECT ('PriGrp_' + @NameSuffix + '_' + CAST(c.n AS varchar) + '_' + CAST(@AccumulateAcc AS varchar)), 'Not Applicable', a.LegacyAccountId,
      NULL, '20050301', NULL, 'AC',
      '123',
      @CreatedBy, '20050301', @ModifiedBy, '20050301',
      enmOrderStatus.Val, ('foo_' + @NameSuffix + '_' + CAST(c.n AS varchar)),CAST(NEWID() AS varchar(50)),CAST(NEWID() AS varchar(50)),
      enmCountry.Val, enmBranch.Val, CAST(c.n AS varchar), ('disp1_' + CAST(c.n AS varchar)), ('disp2_' + CAST(c.n AS varchar)),
      enmSub.Val, (c.n %2), ('po' + @NameSuffix + CAST(c.n AS varchar)), (c.n %2),
      enmSub.Val, ('unameL_' + @NameSuffix + '_' + CAST(c.n AS varchar)), '1'
      FROM mcTesting.timGetNums(1, @iCount) AS c
      INNER JOIN mcTesting.AcctName AS a ON a.ID = ((c.n % @iCountParent) + 1) AND a.AccountTypeID = @iParentAcc
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'OMLockStatus', 'metratech.com/EnumAccount') AS enmOrderStatus ON enmOrderStatus.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'SubsidiaryName', 'metratech.com/EnumGlobal') AS enmSub ON enmSub.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'CountryName', 'Global') AS enmCountry ON enmCountry.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'BranchDepartment', 'metratech.com/EnumGlobal') AS enmBranch ON enmBranch.n = c.n

    SET @AccumulateAcc = @AccumulateAcc + @iCount;
    FETCH NEXT FROM ACC INTO @iParentAcc, @iCount;
  END
CLOSE ACC;
DEALLOCATE ACC;

-- Insert to AcctName
INSERT INTO mcTesting.AcctName (ID, LegacyAccountId, AccountTypeID)
  SELECT ROW_NUMBER() OVER(ORDER BY LegacyAccountId), LegacyAccountId, 14
  FROM mcTesting.PrimaryGroupAcct;


-- iterate through User accounts
SET @AccumulateAcc = 1;
DECLARE ACC CURSOR FAST_FORWARD FOR
  SELECT H.ParentHierID, H.NumAccts
  FROM @HierarchyTypeAcct AS H
  INNER JOIN @AcctType AS A ON A.ID = H.AcctTypeID
  WHERE A.Label = 'UserAccount';
OPEN ACC
FETCH NEXT FROM ACC INTO @iParentAcc, @iCount;
WHILE @@fetch_status = 0
  BEGIN
    SELECT @iCountParent = COUNT(*) FROM mcTesting.AcctName WHERE AccountTypeID = @iParentAcc;

    INSERT INTO mcTesting.UserAcct(LegacyAccountId,LegacySystem,ParentLegacyAccountId,
    PayerLegacyAccountId,StartDate,EndDate,AccountStatus,
    AccountPassword,
    CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,
    OrderStatus,CustPartnerExtId,CustomerNodeName,CommentC,
    LoginU,BillingCodeU,SelfcareRelation,
    SelfcareRole,Moderator,SalesForceIDU,
    UserDispatch1,UserDispatch2,
    EffectiveMgmtSubsidiary,LogoUserName,
    BillingRefU, IsMigrated)
      SELECT ('Usr_' + @NameSuffix + '_' + CAST(c.n AS varchar) + '_' + CAST(@AccumulateAcc AS varchar)), 'Not Applicable', a.LegacyAccountId,
      NULL, '20050301', NULL, 'AC',
      '123',
      @CreatedBy, '20050301', @ModifiedBy, '20050301',
      enmOrderStatus.Val, ('foo_' + @NameSuffix + '_' + CAST(c.n AS varchar)),CAST(NEWID() AS varchar(50)),CAST(NEWID() AS varchar(50)),
      (CAST(c.n AS varchar) + '@email.com'), ('billcode_' + @NameSuffix + '_' + CAST(c.n AS varchar)), enmSelfCRel.Val,
      enmSelfCRole.Val, (c.n % 2), CAST(c.n AS varchar),
      ('udisp1_' + CAST(c.n AS varchar)), ('udisp2_' + CAST(c.n AS varchar)),
      enmSub.Val, ('unameL_' + @NameSuffix + '_' + CAST(c.n AS varchar)),
      ('bru_' + @NameSuffix + '_' + CAST(c.n AS varchar)), '1'
      FROM mcTesting.timGetNums(1, @iCount) AS c
      INNER JOIN mcTesting.AcctName AS a ON a.ID = ((c.n % @iCountParent) + 1) AND a.AccountTypeID = @iParentAcc
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'OMLockStatus', 'metratech.com/EnumAccount') AS enmOrderStatus ON enmOrderStatus.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'SubsidiaryName', 'metratech.com/EnumGlobal') AS enmSub ON enmSub.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'SelfCareRole', 'metratech.com/EnumAccount') AS enmSelfCRole ON enmSelfCRole.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'SelfCareRelation', 'metratech.com/EnumAccount') AS enmSelfCRel ON enmSelfCRel.n = c.n

    SET @AccumulateAcc = @AccumulateAcc + @iCount;
    FETCH NEXT FROM ACC INTO @iParentAcc, @iCount;
  END
CLOSE ACC;
DEALLOCATE ACC;

-- Insert to AcctName
INSERT INTO mcTesting.AcctName (ID, LegacyAccountId, AccountTypeID)
  SELECT ROW_NUMBER() OVER(ORDER BY LegacyAccountId), LegacyAccountId, 18
  FROM mcTesting.UserAcct;


-- iterate through Access accounts
SET @AccumulateAcc = 1;
DECLARE ACC CURSOR FAST_FORWARD FOR
  SELECT H.ParentHierID, H.NumAccts
  FROM @HierarchyTypeAcct AS H
  INNER JOIN @AcctType AS A ON A.ID = H.AcctTypeID
  WHERE A.Label = 'AccessAccount';
OPEN ACC
FETCH NEXT FROM ACC INTO @iParentAcc, @iCount;
WHILE @@fetch_status = 0
  BEGIN
    SELECT @iCountParent = COUNT(*) FROM mcTesting.AcctName WHERE AccountTypeID = @iParentAcc;

    INSERT INTO mcTesting.AccessAcct (LegacyAccountId,LegacySystem,ParentLegacyAccountId,
    PayerLegacyAccountId,StartDate,EndDate,AccountStatus,
    CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,
    OrderStatus,CustPartnerExtId,CustomerNodeName,CommentC,
    AccessCategory,   -- 1
    AccessType,NetworkElementReference,   -- 2
    NetworkElementAccessRef,ExternalConfRef,NetworkElementAccessName,   -- 3
    AudioModeratorPIN,AudioParticipantPIN,   -- 4
    AccessLogin,   -- 5
    AccessPassword,Trial,CommentA,   -- 6
    BillingCode,NetworkElementTechnicalEnv,Topic,  -- 7
    EffectiveMgmtSubsidiary,LogoUserName, IsMigrated)   -- 8
      SELECT ('Access_' + @NameSuffix + '_' + CAST(c.n AS varchar) + '_' + CAST(@AccumulateAcc AS varchar)), 'Not Applicable', a.LegacyAccountId,
      NULL, '20050301', NULL, 'AC',
      @CreatedBy, '20050301', @ModifiedBy, '20050301',
      enmOrderStatus.Val, ('foo_' + @NameSuffix + '_' + CAST(c.n AS varchar)),CAST(NEWID() AS varchar(50)),CAST(NEWID() AS varchar(50)),  
      CASE WHEN (c.n % 2) = 1 THEN 'Web' ELSE 'Audio' END, -- 1
      enmAccType.Val, (@NameSuffix + '_' + CAST(c.n AS varchar)),  -- 2
      'ne' + CAST(c.n AS varchar), 'excon' + CAST(c.n AS varchar), 'accname' + CAST(c.n AS varchar), 
      CAST(c.n AS varchar), CAST(c.n AS varchar),  -- 4
      CASE WHEN (c.n % 2) = 1 THEN ('acc' + @NameSuffix + CAST(c.n AS varchar)) ELSE NULL END,   -- 5
      CAST(c.n AS varchar), (c.n %2), 'foobar!',      -- 6
      CAST(c.n AS varchar), CAST(c.n AS varchar), 'foo.bar',   --7 
      enmSub.Val, ('unameL_' + @NameSuffix + '_' + CAST(c.n AS varchar)), '1'    -- 8
      FROM mcTesting.timGetNums(1, @iCount) AS c
      INNER JOIN mcTesting.AcctName AS a ON a.ID = ((c.n % @iCountParent) + 1) AND a.AccountTypeID = @iParentAcc
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'OMLockStatus', 'metratech.com/EnumAccount') AS enmOrderStatus ON enmOrderStatus.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'SubsidiaryName', 'metratech.com/EnumGlobal') AS enmSub ON enmSub.n = c.n
      INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'AccessType', 'metratech.com/EnumAccount') AS enmAccType ON enmAccType.n = c.n

    SET @AccumulateAcc = @AccumulateAcc + @iCount;
    FETCH NEXT FROM ACC INTO @iParentAcc, @iCount;
  END
CLOSE ACC;
DEALLOCATE ACC;

-- Insert to AcctName
INSERT INTO mcTesting.AcctName (ID, LegacyAccountId, AccountTypeID)
  SELECT ROW_NUMBER() OVER(ORDER BY LegacyAccountId), LegacyAccountId, 7
  FROM mcTesting.AccessAcct;


--- ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
-- Seed data that we need to populate contacts tables
-- NOTE: assumes expected business case where User and Billing accounts are the most populous for which we want to pass contacts!!
DECLARE @iUserCount int;
DECLARE @iBillCount int;
DECLARE @iHighCount int;
SELECT @iUserCount = COUNT(*) FROM mcTesting.AcctName WHERE AccountTypeID = 18;
SELECT @iBillCount = COUNT(*) FROM mcTesting.AcctName WHERE AccountTypeID = 8;
IF @iUserCount > @iBillCount
  SET @iHighCount = @iUserCount
ELSE
  SET @iHighCount = @iBillCount

-- Add some more records to @iHighCount so we have flexibility to move around
SET @iHighCount = @iHighCount + 2000;

-- Get name data
-- Each time we increment GetNums by 1, it increases the number of records by 266,760
INSERT INTO mcTesting.Names (ID, Salutation, FirstName, MiddleInit, LastName)
  SELECT ROW_NUMBER() OVER(ORDER BY X.RndNum) AS r, X.Salutation, X.FirstName, X.MI, x.LastName
  FROM (SELECT s.Salutation, fn.FirstName, mi.MI, ln.LastName, ABS(CAST(NEWID() AS binary(6)) %100000) + 1 AS RndNum
    FROM @FirstNames fn
    OUTER APPLY @MidInits mi
    OUTER APPLY @Salutations s
    OUTER APPLY @LastNames ln
    OUTER APPLY mcTesting.timGetNums(1, ((@iHighCount/266760)+1)) nums) X

INSERT INTO mcTesting.Addresses (ID, [Address], City, Province, Country)
  SELECT ROW_NUMBER() OVER(ORDER BY X.RndNum) AS r, X.[Address], X.City, X.Province, X.Country
  FROM (SELECT CAST(ABS(CAST(NEWID() AS binary(6)) %1000) AS varchar) + ' ' + sn.StreetName + ' ' + ss.Suffix AS [Address], cp.City, cp.Province, cp.Country,
    ABS(CAST(NEWID() AS binary(6)) %100000) + 1 AS RndNum
    FROM @StreetNames sn
    OUTER APPLY @StreetSuffix ss
    OUTER APPLY @CityProvince cp
    OUTER APPLY mcTesting.timGetNums(1, ((@iHighCount/2200)+1)) nums) X

-- Load up Account Contacts   Bill-To, Ship-To, User-Contact
SET @iCount = 0;

-- Need User-Contact for User
SET @iCount = @iUserCount;

INSERT INTO mcTesting.AcctContact (LegacyAccountId,ContactType,CommunicationLanguage,
Timezone,Salutation,Name1,Name2,
FirstName,MiddleInitial,LastName,Email,
PhoneNumber,FacsimileTelephoneNumber,
Address1,Address2,Address3,
City,[State],Zip,Country,
LocalSalutation,LocalFirstName,LocalMiddleInitial,LocalLastName,
LocalAddress1,LocalAddress2,LocalAddress3,LocalCity,LocalState,Company,
AccountTypeId)
  SELECT a.LegacyAccountId, 'User-Contact', enmCusLang.Val,
  enmTZ.Val, nms.Salutation, (nms.FirstName + ' ' + nms.LastName), NULL,
  nms.FirstName, nms.MiddleInit, nms.LastName, (nms.FirstName + '.' + nms.LastName + '@metratech.com'),
  CAST(ABS(CAST(NEWID() AS binary(6)) %1000000000) AS varchar), CAST(ABS(CAST(NEWID() AS binary(6)) %1000000000) AS varchar),
  adr.[Address], CASE WHEN (a.ID % 5) = 4 THEN 'PO Box ' + CAST(a.ID AS varchar) ELSE NULL END, NULL,
  adr.City, adr.Province, CAST(ABS(CAST(NEWID() AS binary(6)) %100000) AS varchar), adr.Country,
  nms.Salutation, nms.FirstName, nms.MiddleInit, nms.LastName,
  adr.[Address], NULL, NULL, adr.City, adr.Province, CAST(NEWID() AS varchar(50)),
  a.AccountTypeID
  FROM mcTesting.AcctName AS a
  INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'CustomLanguageCode', 'metratech.com/EnumAccount') AS enmCusLang ON enmCusLang.n = a.ID
  INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'TimeZoneID', 'Global') AS enmTZ ON enmTZ.n = a.ID
  INNER JOIN mcTesting.Addresses AS adr ON adr.ID = a.ID
  INNER JOIN mcTesting.Names AS nms ON nms.ID = a.ID
  WHERE a.AccountTypeID = 18;


-- Need Bill-To contact for Legal Entity, Billing, and Partner
SET @iCount = @iBillCount;

INSERT INTO mcTesting.AcctContact (LegacyAccountId,ContactType,CommunicationLanguage,
Timezone,Salutation,Name1,Name2,
FirstName,MiddleInitial,LastName,Email,
PhoneNumber,FacsimileTelephoneNumber,
Address1,Address2,Address3,
City,[State],Zip,Country,
LocalSalutation,LocalFirstName,LocalMiddleInitial,LocalLastName,
LocalAddress1,LocalAddress2,LocalAddress3,LocalCity,LocalState,Company,
AccountTypeId)
  SELECT a.LegacyAccountId, 'Bill-To', enmCusLang.Val,
  enmTZ.Val, nms.Salutation, (nms.FirstName + ' ' + nms.LastName), NULL,
  nms.FirstName, nms.MiddleInit, nms.LastName, (nms.FirstName + '.' + nms.LastName + '@metratech.com'),
  CAST(ABS(CAST(NEWID() AS binary(6)) %1000000000) AS varchar), CAST(ABS(CAST(NEWID() AS binary(6)) %1000000000) AS varchar),
  adr.[Address], CASE WHEN (a.ID % 5) = 4 THEN 'PO Box ' + CAST(a.ID AS varchar) ELSE NULL END, NULL,
  adr.City, adr.Province, CAST(ABS(CAST(NEWID() AS binary(6)) %100000) AS varchar), adr.Country,
  nms.Salutation, nms.FirstName, nms.MiddleInit, nms.LastName,
  adr.[Address], NULL, NULL, adr.City, adr.Province, CAST(NEWID() AS varchar(50)),
  a.AccountTypeID
  FROM mcTesting.AcctName AS a
  INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'CustomLanguageCode', 'metratech.com/EnumAccount') AS enmCusLang ON enmCusLang.n = a.ID
  INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'TimeZoneID', 'Global') AS enmTZ ON enmTZ.n = a.ID
  INNER JOIN mcTesting.Addresses AS adr ON adr.ID = a.ID
  INNER JOIN mcTesting.Names AS nms ON nms.ID = a.ID
  WHERE a.AccountTypeID IN (8,10,13);


-- Need Ship-To for Billing and Partner
SET @iCount = @iBillCount;

INSERT INTO mcTesting.AcctContact (LegacyAccountId,ContactType,CommunicationLanguage,
Timezone,Salutation,Name1,Name2,
FirstName,MiddleInitial,LastName,Email,
PhoneNumber,FacsimileTelephoneNumber,
Address1,Address2,Address3,
City,[State],Zip,Country,
LocalSalutation,LocalFirstName,LocalMiddleInitial,LocalLastName,
LocalAddress1,LocalAddress2,LocalAddress3,LocalCity,LocalState,Company,
AccountTypeId)
  SELECT a.LegacyAccountId, 'Ship-To', enmCusLang.Val,
  enmTZ.Val, nms.Salutation, (nms.FirstName + ' ' + nms.LastName), NULL,
  nms.FirstName, nms.MiddleInit, nms.LastName, (nms.FirstName + '.' + nms.LastName + '@metratech.com'),
  CAST(ABS(CAST(NEWID() AS binary(6)) %1000000000) AS varchar), CAST(ABS(CAST(NEWID() AS binary(6)) %1000000000) AS varchar),
  adr.[Address], CASE WHEN (a.ID % 5) = 4 THEN 'PO Box ' + CAST(a.ID AS varchar) ELSE NULL END, NULL,
  adr.City, adr.Province, CAST(ABS(CAST(NEWID() AS binary(6)) %100000) AS varchar), adr.Country,
  nms.Salutation, nms.FirstName, nms.MiddleInit, nms.LastName,
  adr.[Address], NULL, NULL, adr.City, adr.Province, CAST(NEWID() AS varchar(50)),
  a.AccountTypeID
  FROM mcTesting.AcctName AS a
  INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'CustomLanguageCode', 'metratech.com/EnumAccount') AS enmCusLang ON enmCusLang.n = a.ID
  INNER JOIN mcTesting.GetEnumBracket(1, @iCount, 'TimeZoneID', 'Global') AS enmTZ ON enmTZ.n = a.ID
  INNER JOIN mcTesting.Addresses AS adr ON adr.ID = a.ID
  INNER JOIN mcTesting.Names AS nms ON nms.ID = a.ID
  WHERE a.AccountTypeID IN (8,13);


-- ####################################################################################################
-- ####################################################################################################
-- Now BCP the files
-- Specify where you want to drop the output files, and bcp parameters
-- Note you cannot output in UTF-8 format from bcp; you can only do UTF-16 or ANSI
DECLARE @OutputDir varchar(200) = 'C:\mcfoo\';
DECLARE @bcp varchar(500) = ' -w -k -T -S"localhost" -t"`|" -r"<\n"';
--DECLARE @bcp varchar(500) = ' -C ACP -k -T -S"localhost" -t"`|" -r"<\n"';
DECLARE @bcp_push varchar(4000);

-- Opt to include only certain account types. (This is to facilitate testing where we break up the files.) Note this is only 
-- necessary for accounts, where there is a dependency on the AccountContact file for the load to run.
DECLARE @doAccess bit = 1;
DECLARE @doBilling bit = 1;
DECLARE @doGroup bit = 1;
DECLARE @doLegalEntity bit = 1;
DECLARE @doLocalLogo bit = 1;
DECLARE @doLogo bit = 1;
DECLARE @doPartner bit = 1;
DECLARE @doPrimaryGroup bit = 1;
DECLARE @doRG1 bit = 1;
DECLARE @doRG2 bit = 1;
DECLARE @doUser bit = 1;
DECLARE @doSilo bit = 1;

-- Access
IF @doAccess = 1
  BEGIN
    SELECT @bcp_push = 'BCP "select * from NetMeter.mcTesting.AccessAcct" queryout ' + @OutputDir + 'AccessAccount.MN.1.data' + @bcp;
    EXEC sys.xp_cmdshell @bcp_push;
  END

-- Billing
IF @doBilling = 1
  BEGIN
    SELECT @bcp_push = 'BCP "select * from NetMeter.mcTesting.BillingAcct" queryout ' + @OutputDir + 'BillingAccount.MN.1.data' + @bcp;
    EXEC sys.xp_cmdshell @bcp_push;
  END

-- Group
IF @doGroup = 1
  BEGIN
    SELECT @bcp_push = 'BCP "select * from NetMeter.mcTesting.GroupAcct" queryout ' + @OutputDir + 'GroupAccount.MN.1.data' + @bcp;
    EXEC sys.xp_cmdshell @bcp_push;
  END

-- Legal Entity
IF @doLegalEntity = 1
  BEGIN
    SELECT @bcp_push = 'BCP "select * from NetMeter.mcTesting.LegalEntityAcct" queryout ' + @OutputDir + 'LegalEntityAccount.MN.1.data' + @bcp;
    EXEC sys.xp_cmdshell @bcp_push;
  END

-- Local Logo
IF @doLocalLogo = 1
  BEGIN
    SELECT @bcp_push = 'BCP "select * from NetMeter.mcTesting.LocalLogoAcct" queryout ' + @OutputDir + 'LocalLogoAccount.MN.1.data' + @bcp;
    EXEC sys.xp_cmdshell @bcp_push;
  END

-- Logo
IF @doLogo = 1
  BEGIN
    SELECT @bcp_push = 'BCP "select * from NetMeter.mcTesting.LogoAcct" queryout ' + @OutputDir + 'LogoAccount.MN.1.data' + @bcp;
    EXEC sys.xp_cmdshell @bcp_push;
  END

-- Partner
IF @doPartner = 1
  BEGIN
    SELECT @bcp_push = 'BCP "select * from NetMeter.mcTesting.PartnerAcct" queryout ' + @OutputDir + 'PartnerAccount.MN.1.data' + @bcp;
    EXEC sys.xp_cmdshell @bcp_push;
  END

-- Primary Group
IF @doPrimaryGroup = 1
  BEGIN
    SELECT @bcp_push = 'BCP "select * from NetMeter.mcTesting.PrimaryGroupAcct" queryout ' + @OutputDir + 'PrimaryGroupAccount.MN.1.data' + @bcp;
    EXEC sys.xp_cmdshell @bcp_push;
  END

-- Reporting Group 1
IF @doRG1 = 1
  BEGIN
    SELECT @bcp_push = 'BCP "select * from NetMeter.mcTesting.RepGrpOneAcct" queryout ' + @OutputDir + 'ReportingGroup1Account.MN.1.data' + @bcp;
    EXEC sys.xp_cmdshell @bcp_push;
  END

-- Reporting Group 2
IF @doRG2 = 1
  BEGIN
    SELECT @bcp_push = 'BCP "select * from NetMeter.mcTesting.RepGrpTwoAcct" queryout ' + @OutputDir + 'ReportingGroup2Account.MN.1.data' + @bcp;
    EXEC sys.xp_cmdshell @bcp_push;
  END

-- User
IF @doUser = 1
  BEGIN
    SELECT @bcp_push = 'BCP "select * from NetMeter.mcTesting.UserAcct" queryout ' + @OutputDir + 'UserAccount.MN.1.data' + @bcp;
    EXEC sys.xp_cmdshell @bcp_push;
  END

-- Silo
IF @doSilo = 1
  BEGIN
    SELECT @bcp_push = 'BCP "select * from NetMeter.mcTesting.SiloAcct" queryout ' + @OutputDir + 'SiloAccount.MN.1.data' + @bcp;
    EXEC sys.xp_cmdshell @bcp_push;
  END


-- Account Contact and Account Name (alias): build a string to support desired filters
DECLARE @strInClause varchar(1000);
DECLARE @sqlData varchar(4000);

IF @doAccess = 1 AND @doBilling = 1 AND @doGroup = 1 AND @doLegalEntity = 1 AND @doLocalLogo = 1 AND @doLogo = 1 AND @doPartner = 1 AND @doPrimaryGroup = 1
  AND @doRG1 = 1 AND @doRG2 = 1 AND @doUser = 1 AND @doSilo = 1
  BEGIN
    SET @strInClause = '';
  END
ELSE
  BEGIN
    SET @strInClause = ' WHERE AccountTypeId IN (';

    IF @doAccess = 1 SET @strInClause = @strInClause + '7,';
    IF @doBilling = 1 SET @strInClause = @strInClause + '8,';
    IF @doGroup = 1 SET @strInClause = @strInClause + '9,';
    IF @doLegalEntity = 1 SET @strInClause = @strInClause + '10,';
    IF @doLocalLogo = 1 SET @strInClause = @strInClause + '11,';
    IF @doLogo = 1 SET @strInClause = @strInClause + '12,';
    IF @doPartner = 1 SET @strInClause = @strInClause + '13,';
    IF @doPrimaryGroup = 1 SET @strInClause = @strInClause + '14,';
    IF @doRG1 = 1 SET @strInClause = @strInClause + '15,';
    IF @doRG2 = 1 SET @strInClause = @strInClause + '16,';
    IF @doUser = 1 SET @strInClause = @strInClause + '18,';
    IF @doSilo = 1 SET @strInClause = @strInClause + '17,';
    
    SET @strInClause = @strInClause + '0)';
  END;

-- Account Name/Alias
IF @doAccess = 1
  BEGIN
    SET @sqlData = 'SELECT LegacyAccountId, CASE WHEN AccessCategory = ''Web'' THEN LegacyAccountId + ''Web'' ELSE LegacyAccountId + ''Audio'' END,';
    SET @sqlData = @sqlData + ' CASE WHEN AccessCategory = ''Web'' THEN ''WebConnectionAliasType'' ELSE ''AudioConnectionAliasType'' END ';
    SET @sqlData = @sqlData + ' FROM NetMeter.mcTesting.AccessAcct' ;
print @sqlData;

    SELECT @bcp_push = 'BCP "' + @sqlData + '" queryout ' + @OutputDir + 'AccountName.MN.1.data' + @bcp;
    EXEC sys.xp_cmdshell @bcp_push;
  END;

-- Account Contact
SET @sqlData = 'SELECT LegacyAccountId, ContactType, CommunicationLanguage, Timezone, Salutation, Name1, Name2, FirstName, MiddleInitial,';
SET @sqlData = @sqlData + 'LastName, Email, PhoneNumber, FacsimileTelephoneNumber, Address1, Address2, Address3, City, [State], Zip, Country, LocalSalutation,';
SET @sqlData = @sqlData + 'LocalFirstName, LocalMiddleInitial, LocalLastName, LocalAddress1, LocalAddress2, LocalAddress3, LocalCity, LocalState, Company';
SET @sqlData = @sqlData + ' FROM NetMeter.mcTesting.AcctContact ' + @strInClause;

SELECT @bcp_push = 'BCP "' + @sqlData + '" queryout ' + @OutputDir + 'AccountContact.MN.1.data' + @bcp;
EXEC sys.xp_cmdshell @bcp_push;

PRINT @bcp_push

-- ####################################################################################################
-- ####################################################################################################



/*
FINAL CLEANUP

IF OBJECT_ID('mcTesting.timGetNums', 'IF') IS NOT NULL DROP FUNCTION mcTesting.timGetNums;
GO
IF OBJECT_ID('mcTesting.timGetEnum', 'FN') IS NOT NULL DROP FUNCTION mcTesting.timGetEnum;
GO

IF EXISTS(SELECT * FROM sys.schemas WHERE name = 'mcTesting') 
DROP SCHEMA mcTesting;
GO
*/
