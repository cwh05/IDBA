﻿USE master
GO

-- Create database
IF (NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'AMS_DATABASE'))
BEGIN
   CREATE DATABASE AMS_DATABASE;
   PRINT 'Database created...'
END
GO

USE AMS_DATABASE;
GO
SET NOCOUNT ON

-- Table that consists of a list of roles
CREATE TABLE dbo.RoleCategory
(
   RoleID TINYINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
   RoleTitle NVARCHAR(100) NOT NULL,

   CreatedDate SMALLDATETIME NOT NULL,
);

CREATE TABLE dbo.Country
(
   CountryCode NVARCHAR(5) NOT NULL PRIMARY KEY,
   CountryTitle NVARCHAR(100) NOT NULL
);

CREATE TABLE dbo.Account
(
   AccountID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
   LoginUsername NVARCHAR(100) NOT NULL UNIQUE,
   LoginPassword NVARCHAR(255) NOT NULL
);

CREATE TABLE dbo.Employee
(
   EmployeeID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
   EmployeeFirstName NVARCHAR(150) NOT NULL,
   EmployeeLastName NVARCHAR(100) NOT NULL,
   Gender BIT NOT NULL,          -- 0=male ; 1=female
   DateOfBirth DATE NOT NULL,

   Address1 NVARCHAR(100) NOT NULL,
   Address2 NVARCHAR(100) NULL,
   City NVARCHAR(100) NOT NULL,
   PostCode NVARCHAR(20) NOT NULL,
   StateProvince NVARCHAR(80) NOT NULL,
   CountryCode NVARCHAR(5) NOT NULL,

   ContactNumber NVARCHAR(15) NOT NULL,
   Email NVARCHAR(255) NOT NULL,

   AccountID INT NULL,
   RoleID TINYINT NULL,

   CreatedDate SMALLDATETIME NOT NULL,
   ModifiedDate SMALLDATETIME NOT NULL,    -- ModifiedDate = CreatedDate for the first time

   CONSTRAINT fk_EmployeeAccount FOREIGN KEY(AccountID) REFERENCES Account(AccountID),
   CONSTRAINT fk_EmployeeCountry FOREIGN KEY(CountryCode) REFERENCES Country(CountryCode),
   CONSTRAINT fk_RoleCategory FOREIGN KEY(RoleID) REFERENCES RoleCategory(RoleID)
);

CREATE TABLE dbo.Program
(
   ProgramID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
   ProgramName NVARCHAR(200) NOT NULL,
   ProgramDescription NVARCHAR(MAX) NULL,
   ManagerID INT NULL,

   CreatedDate SMALLDATETIME NOT NULL,
   ModifiedDate SMALLDATETIME NOT NULL,   -- ModifiedDate = CreatedDate for the first time

   CONSTRAINT fk_employee FOREIGN KEY(ManagerID) REFERENCES Employee(EmployeeID)
);

CREATE TABLE dbo.Student
(
   StudentID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
   StudentFirstName NVARCHAR(150) NOT NULL,
   StudentLastName NVARCHAR(100) NOT NULL,
   Gender BIT NOT NULL,          -- 0=male ; 1=female
   DateOfBirth DATE NOT NULL,

   Address1 NVARCHAR(100) NOT NULL,
   Address2 NVARCHAR(100) NULL,
   City NVARCHAR(100) NOT NULL,
   PostCode NVARCHAR(20) NOT NULL,
   StateProvince NVARCHAR(80) NOT NULL,
   CountryCode NVARCHAR(5) NOT NULL,

   ContactNumber NVARCHAR(15) NOT NULL,
   Email NVARCHAR(255) NOT NULL,

   AccountID INT NULL,
   ProgramID INT NULL,

   CreatedDate SMALLDATETIME NOT NULL,
   ModifiedDate SMALLDATETIME NOT NULL,    -- ModifiedDate = CreatedDate for the first time

   CONSTRAINT fk_StudentAccount FOREIGN KEY(AccountID) REFERENCES Account(AccountID),
   CONSTRAINT fk_StudentCountry FOREIGN KEY(CountryCode) REFERENCES Country(CountryCode),
   CONSTRAINT fk_StudentProgram FOREIGN KEY(ProgramID) REFERENCES Program(ProgramID)
);

CREATE TABLE dbo.Course
(
   CourseID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
   CourseName NVARCHAR(200) NOT NULL,
   CourseCode NVARCHAR(60) NOT NULL UNIQUE,
   CourseDescription NVARCHAR(MAX) NULL,
   StaffID INT NULL,
   CreatedDate SMALLDATETIME NOT NULL,
   ModifiedDate SMALLDATETIME NOT NULL    -- ModifiedDate = CreatedDate for the first time

   CONSTRAINT fk_Staff FOREIGN KEY(StaffID) REFERENCES Employee(EmployeeID)
);

CREATE TABLE dbo.Department
(
   DepartmentID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
   DepartmentName NVARCHAR(200) NOT NULL,

   CreatedDate SMALLDATETIME NOT NULL,
   ModifiedDate SMALLDATETIME NOT NULL    -- ModifiedDate = CreatedDate for the first time
);

-- Table for student to add/remove courses
CREATE TABLE dbo.StudentCourse
(
   StudentID INT NOT NULL,
   CourseID INT NOT NULL,
   Marks FLOAT NULL,
   CreatedDate SMALLDATETIME NOT NULL,

   CONSTRAINT fk_Student FOREIGN KEY(StudentID) REFERENCES Student(StudentID) ON DELETE CASCADE,
   CONSTRAINT fk_Course FOREIGN KEY(CourseID) REFERENCES Course(CourseID)
);

CREATE TABLE dbo.EmployeeDepartment
(
   DepartmentID INT NOT NULL FOREIGN KEY REFERENCES Department(DepartmentID),
   EmployeeID INT NOT NULL FOREIGN KEY REFERENCES Employee(EmployeeID) ON DELETE CASCADE
);

CREATE TABLE dbo.ProgramCourse
(
   ProgramID INT NOT NULL FOREIGN KEY REFERENCES Program(ProgramID),
   CourseID INT NOT NULL FOREIGN KEY REFERENCES Course(CourseID) ON DELETE CASCADE
);
PRINT 'Tables created...'
GO

-- Rule for dbo.StudentCourse's marks
CREATE RULE CourseMarksRule AS @Marks >=0.0 AND @Marks <=100.0
GO
EXEC sp_bindrule 'CourseMarksRule', 'StudentCourse.Marks'
PRINT 'Rules created...'
GO

-- Default value for CreatedDate attribute
CREATE DEFAULT DATETIMECREATED AS CURRENT_TIMESTAMP
GO
sp_bindefault 'DATETIMECREATED', 'dbo.Department.CreatedDate'
GO
sp_bindefault 'DATETIMECREATED', 'dbo.Course.CreatedDate'
GO
sp_bindefault 'DATETIMECREATED', 'dbo.Student.CreatedDate'
GO
sp_bindefault 'DATETIMECREATED', 'dbo.Employee.CreatedDate'
GO
sp_bindefault 'DATETIMECREATED', 'dbo.RoleCategory.CreatedDate'
GO
sp_bindefault 'DATETIMECREATED', 'dbo.Program.CreatedDate'
GO
sp_bindefault 'DATETIMECREATED', 'dbo.StudentCourse.CreatedDate'
GO
PRINT 'Default created...'
GO

CREATE VIEW Enrollment
AS
SELECT s.StudentID, s.StudentFirstName, s.StudentLastName, s.Email, c.CourseID,
      c.CourseName, c.CourseCode, sc.Marks FROM StudentCourse sc
      INNER JOIN Student s ON s.StudentID = sc.StudentID
      INNER JOIN Course c ON c.CourseID = sc.CourseID
GO
PRINT 'View created...'
GO

-- Trigger: Student enrols/delete 1 course
CREATE TRIGGER InsertDeleteStudentCourse ON Enrollment
   INSTEAD OF INSERT, DELETE
AS
   BEGIN
      IF EXISTS(SELECT * FROM DELETED)
         DELETE FROM StudentCourse WHERE StudentID = (SELECT StudentID FROM DELETED)
            AND CourseID = (SELECT CourseID FROM DELETED)
      ELSE
         INSERT INTO StudentCourse(StudentID, CourseID)
            SELECT StudentID, CourseID FROM INSERTED
   END
GO

-- Trigger: Staff update or insert marks of a course enrolled by student
-- When updating the Enrollment view, pls supply StudentID and CourseID
CREATE TRIGGER UpdateStudentCourseScore ON Enrollment
   INSTEAD OF UPDATE
AS
   BEGIN
      UPDATE StudentCourse SET Marks = (SELECT Marks FROM INSERTED)
         WHERE StudentID = (SELECT StudentID FROM INSERTED) AND
            CourseID = (SELECT CourseID FROM INSERTED)
   END
GO

CREATE TRIGGER [dbo].[AvgScorePerCourse] ON [dbo].[StudentCourse]
   FOR INSERT, UPDATE
AS
   DECLARE @v INT
   DECLARE @cid INT

   BEGIN
      SET @cid = (SELECT CourseID FROM INSERTED)
      SET @v = (SELECT COUNT(StudentID) FROM Enrollment WHERE CourseID = @cid
                     AND Marks IS NOT NULL)

      IF (@v % 5) = 0
         SELECT CAST(AVG(Marks) AS varchar(10)) AS 'Average' FROM Enrollment WHERE CourseID = @cid
               AND Marks IS NOT NULL
      ELSE
         SELECT '-1.0' AS 'Average'
   END
GO
PRINT 'Triggers created...'
GO

CREATE UNIQUE INDEX IX_LOGIN ON Account(LoginUsername)
CREATE UNIQUE INDEX IX_COURSE ON Course(CourseCode)
PRINT 'Indexes created...'
GO


BEGIN
   -- Insert data into RoleCategory table
   INSERT INTO dbo.RoleCategory(RoleTitle) VALUES('Administrator')
   INSERT INTO dbo.RoleCategory(RoleTitle) VALUES('Program Manager')
   INSERT INTO dbo.RoleCategory(RoleTitle) VALUES('Staff')
   PRINT 'Data inserted to dbo.RoleCategory...'

   -- Insert data into Country Table
   INSERT INTO Country VALUES('AF', N'Afghanistan')
   INSERT INTO Country VALUES('AX', N'Åland Islands')
   INSERT INTO Country VALUES('AL', N'Albania')
   INSERT INTO Country VALUES('DZ', N'Algeria')
   INSERT INTO Country VALUES('AS', N'American Samoa')
   INSERT INTO Country VALUES('AD', N'Andorra')
   INSERT INTO Country VALUES('AO', N'Angola')
   INSERT INTO Country VALUES('AI', N'Anguilla')
   INSERT INTO Country VALUES('AQ', N'Antarctica')
   INSERT INTO Country VALUES('AG', N'Antigua and Barbuda')
   INSERT INTO Country VALUES('AR', N'Argentina')
   INSERT INTO Country VALUES('AM', N'Armenia')
   INSERT INTO Country VALUES('AW', N'Aruba')
   INSERT INTO Country VALUES('AU', N'Australia')
   INSERT INTO Country VALUES('AT', N'Austria')
   INSERT INTO Country VALUES('AZ', N'Azerbaijan')
   INSERT INTO Country VALUES('BS', N'Bahamas')
   INSERT INTO Country VALUES('BH', N'Bahrain')
   INSERT INTO Country VALUES('BD', N'Bangladesh')
   INSERT INTO Country VALUES('BB', N'Barbados')
   INSERT INTO Country VALUES('BY', N'Belarus')
   INSERT INTO Country VALUES('BE', N'Belgium')
   INSERT INTO Country VALUES('BZ', N'Belize')
   INSERT INTO Country VALUES('BJ', N'Benin')
   INSERT INTO Country VALUES('BM', N'Bermuda')
   INSERT INTO Country VALUES('BT', N'Bhutan')
   INSERT INTO Country VALUES('BO', N'Bolivia')
   INSERT INTO Country VALUES('BQ', N'Bonaire')
   INSERT INTO Country VALUES('BA', N'Bosnia and Herzegovina')
   INSERT INTO Country VALUES('BW', N'Botswana')
   INSERT INTO Country VALUES('BV', N'Bouvet Island')
   INSERT INTO Country VALUES('BR', N'Brazil')
   INSERT INTO Country VALUES('IO', N'British Indian Ocean Territory')
   INSERT INTO Country VALUES('BN', N'Brunei')
   INSERT INTO Country VALUES('BG', N'Bulgaria')
   INSERT INTO Country VALUES('BF', N'Burkina Faso')
   INSERT INTO Country VALUES('BI', N'Burundi')
   INSERT INTO Country VALUES('KH', N'Cambodia')
   INSERT INTO Country VALUES('CM', N'Cameroon')
   INSERT INTO Country VALUES('CA', N'Canada')
   INSERT INTO Country VALUES('CV', N'Cape Verde')
   INSERT INTO Country VALUES('KY', N'Cayman Islands')
   INSERT INTO Country VALUES('CF', N'Central African Republic')
   INSERT INTO Country VALUES('TD', N'Chad')
   INSERT INTO Country VALUES('CL', N'Chile')
   INSERT INTO Country VALUES('CN', N'China')
   INSERT INTO Country VALUES('CX', N'Christmas Island')
   INSERT INTO Country VALUES('CC', N'Cocos (Keeling) Islands')
   INSERT INTO Country VALUES('CO', N'Colombia')
   INSERT INTO Country VALUES('KM', N'Comoros')
   INSERT INTO Country VALUES('CG', N'Congo')
   INSERT INTO Country VALUES('CD', N'Congo (DRC)')
   INSERT INTO Country VALUES('CK', N'Cook Islands')
   INSERT INTO Country VALUES('CR', N'Costa Rica')
   INSERT INTO Country VALUES('HR', N'Croatia')
   INSERT INTO Country VALUES('CU', N'Cuba')
   INSERT INTO Country VALUES('CW', N'Curaçao')
   INSERT INTO Country VALUES('CY', N'Cyprus')
   INSERT INTO Country VALUES('CZ', N'Czech Republic')
   INSERT INTO Country VALUES('DK', N'Denmark')
   INSERT INTO Country VALUES('DJ', N'Djibouti')
   INSERT INTO Country VALUES('DM', N'Dominica')
   INSERT INTO Country VALUES('DO', N'Dominican Republic')
   INSERT INTO Country VALUES('EC', N'Ecuador')
   INSERT INTO Country VALUES('EG', N'Egypt')
   INSERT INTO Country VALUES('SV', N'El Salvador')
   INSERT INTO Country VALUES('GQ', N'Equatorial Guinea')
   INSERT INTO Country VALUES('ER', N'Eritrea')
   INSERT INTO Country VALUES('EE', N'Estonia')
   INSERT INTO Country VALUES('ET', N'Ethiopia')
   INSERT INTO Country VALUES('FK', N'Falkland Islands (Islas Malvinas)')
   INSERT INTO Country VALUES('FO', N'Faroe Islands')
   INSERT INTO Country VALUES('FJ', N'Fiji Islands')
   INSERT INTO Country VALUES('FI', N'Finland')
   INSERT INTO Country VALUES('FR', N'France')
   INSERT INTO Country VALUES('GF', N'French Guiana')
   INSERT INTO Country VALUES('PF', N'French Polynesia')
   INSERT INTO Country VALUES('TF', N'French Southern and Antarctic Lands')
   INSERT INTO Country VALUES('GA', N'Gabon')
   INSERT INTO Country VALUES('GM', N'Gambia, The')
   INSERT INTO Country VALUES('GE', N'Georgia')
   INSERT INTO Country VALUES('DE', N'Germany')
   INSERT INTO Country VALUES('GH', N'Ghana')
   INSERT INTO Country VALUES('GI', N'Gibraltar')
   INSERT INTO Country VALUES('GR', N'Greece')
   INSERT INTO Country VALUES('GL', N'Greenland')
   INSERT INTO Country VALUES('GD', N'Grenada')
   INSERT INTO Country VALUES('GP', N'Guadeloupe')
   INSERT INTO Country VALUES('GU', N'Guam')
   INSERT INTO Country VALUES('GT', N'Guatemala')
   INSERT INTO Country VALUES('GG', N'Guernsey')
   INSERT INTO Country VALUES('GN', N'Guinea')
   INSERT INTO Country VALUES('GW', N'Guinea-Bissau')
   INSERT INTO Country VALUES('GY', N'Guyana')
   INSERT INTO Country VALUES('HT', N'Haiti')
   INSERT INTO Country VALUES('HM', N'Heard Island and McDonald Islands')
   INSERT INTO Country VALUES('HN', N'Honduras')
   INSERT INTO Country VALUES('HK', N'Hong Kong SAR')
   INSERT INTO Country VALUES('HU', N'Hungary')
   INSERT INTO Country VALUES('IS', N'Iceland')
   INSERT INTO Country VALUES('IN', N'India')
   INSERT INTO Country VALUES('ID', N'Indonesia')
   INSERT INTO Country VALUES('IR', N'Iran')
   INSERT INTO Country VALUES('IQ', N'Iraq')
   INSERT INTO Country VALUES('IE', N'Ireland')
   INSERT INTO Country VALUES('IM', N'Isle of Man')
   INSERT INTO Country VALUES('IL', N'Israel')
   INSERT INTO Country VALUES('IT', N'Italy')
   INSERT INTO Country VALUES('JM', N'Jamaica')
   INSERT INTO Country VALUES('SJ', N'Jan Mayen')
   INSERT INTO Country VALUES('JP', N'Japan')
   INSERT INTO Country VALUES('JE', N'Jersey')
   INSERT INTO Country VALUES('JO', N'Jordan')
   INSERT INTO Country VALUES('KZ', N'Kazakhstan')
   INSERT INTO Country VALUES('KE', N'Kenya')
   INSERT INTO Country VALUES('KI', N'Kiribati')
   INSERT INTO Country VALUES('KR', N'Korea')
   INSERT INTO Country VALUES('KW', N'Kuwait')
   INSERT INTO Country VALUES('KG', N'Kyrgyzstan')
   INSERT INTO Country VALUES('LA', N'Laos')
   INSERT INTO Country VALUES('LV', N'Latvia')
   INSERT INTO Country VALUES('LB', N'Lebanon')
   INSERT INTO Country VALUES('LS', N'Lesotho')
   INSERT INTO Country VALUES('LR', N'Liberia')
   INSERT INTO Country VALUES('LY', N'Libya')
   INSERT INTO Country VALUES('LI', N'Liechtenstein')
   INSERT INTO Country VALUES('LT', N'Lithuania')
   INSERT INTO Country VALUES('LU', N'Luxembourg')
   INSERT INTO Country VALUES('MO', N'Macao SAR')
   INSERT INTO Country VALUES('MK', N'Macedonia, Former Yugoslav Republic of')
   INSERT INTO Country VALUES('MG', N'Madagascar')
   INSERT INTO Country VALUES('MW', N'Malawi')
   INSERT INTO Country VALUES('MY', N'Malaysia')
   INSERT INTO Country VALUES('MV', N'Maldives')
   INSERT INTO Country VALUES('ML', N'Mali')
   INSERT INTO Country VALUES('MT', N'Malta')
   INSERT INTO Country VALUES('MH', N'Marshall Islands')
   INSERT INTO Country VALUES('MQ', N'Martinique')
   INSERT INTO Country VALUES('MR', N'Mauritania')
   INSERT INTO Country VALUES('MU', N'Mauritius')
   INSERT INTO Country VALUES('YT', N'Mayotte')
   INSERT INTO Country VALUES('MX', N'Mexico')
   INSERT INTO Country VALUES('FM', N'Micronesia')
   INSERT INTO Country VALUES('MD', N'Moldova')
   INSERT INTO Country VALUES('MC', N'Monaco')
   INSERT INTO Country VALUES('MN', N'Mongolia')
   INSERT INTO Country VALUES('ME', N'Montenegro')
   INSERT INTO Country VALUES('MS', N'Montserrat')
   INSERT INTO Country VALUES('MA', N'Morocco')
   INSERT INTO Country VALUES('MZ', N'Mozambique')
   INSERT INTO Country VALUES('MM', N'Myanmar')
   INSERT INTO Country VALUES('NA', N'Namibia')
   INSERT INTO Country VALUES('NR', N'Nauru')
   INSERT INTO Country VALUES('NP', N'Nepal')
   INSERT INTO Country VALUES('NL', N'Netherlands')
   INSERT INTO Country VALUES('NC', N'New Caledonia')
   INSERT INTO Country VALUES('NZ', N'New Zealand')
   INSERT INTO Country VALUES('NI', N'Nicaragua')
   INSERT INTO Country VALUES('NE', N'Niger')
   INSERT INTO Country VALUES('NG', N'Nigeria')
   INSERT INTO Country VALUES('NU', N'Niue')
   INSERT INTO Country VALUES('NF', N'Norfolk Island')
   INSERT INTO Country VALUES('KP', N'North Korea')
   INSERT INTO Country VALUES('MP', N'Northern Mariana Islands')
   INSERT INTO Country VALUES('NO', N'Norway')
   INSERT INTO Country VALUES('OM', N'Oman')
   INSERT INTO Country VALUES('PK', N'Pakistan')
   INSERT INTO Country VALUES('PW', N'Palau')
   INSERT INTO Country VALUES('PS', N'Palestinian Authority')
   INSERT INTO Country VALUES('PA', N'Panama')
   INSERT INTO Country VALUES('PG', N'Papua New Guinea')
   INSERT INTO Country VALUES('PY', N'Paraguay')
   INSERT INTO Country VALUES('PE', N'Peru')
   INSERT INTO Country VALUES('PH', N'Philippines')
   INSERT INTO Country VALUES('PN', N'Pitcairn Islands')
   INSERT INTO Country VALUES('PL', N'Poland')
   INSERT INTO Country VALUES('PT', N'Portugal')
   INSERT INTO Country VALUES('PR', N'Puerto Rico')
   INSERT INTO Country VALUES('QA', N'Qatar')
   INSERT INTO Country VALUES('CI', N'Republic of Côte d''Ivoire')
   INSERT INTO Country VALUES('RE', N'Reunion')
   INSERT INTO Country VALUES('RO', N'Romania')
   INSERT INTO Country VALUES('RU', N'Russia')
   INSERT INTO Country VALUES('RW', N'Rwanda')
   INSERT INTO Country VALUES('XS', N'Saba')
   INSERT INTO Country VALUES('WS', N'Samoa')
   INSERT INTO Country VALUES('SM', N'San Marino')
   INSERT INTO Country VALUES('ST', N'São Tomé and Príncipe')
   INSERT INTO Country VALUES('SA', N'Saudi Arabia')
   INSERT INTO Country VALUES('SN', N'Senegal')
   INSERT INTO Country VALUES('RS', N'Serbia')
   INSERT INTO Country VALUES('SC', N'Seychelles')
   INSERT INTO Country VALUES('SL', N'Sierra Leone')
   INSERT INTO Country VALUES('SG', N'Singapore')
   INSERT INTO Country VALUES('XE', N'Sint Eustatius')
   INSERT INTO Country VALUES('SX', N'Sint Maarten')
   INSERT INTO Country VALUES('SK', N'Slovakia')
   INSERT INTO Country VALUES('SI', N'Slovenia')
   INSERT INTO Country VALUES('SB', N'Solomon Islands')
   INSERT INTO Country VALUES('SO', N'Somalia')
   INSERT INTO Country VALUES('ZA', N'South Africa')
   INSERT INTO Country VALUES('GS', N'South Georgia and the South Sandwich Islands')
   INSERT INTO Country VALUES('ES', N'Spain')
   INSERT INTO Country VALUES('LK', N'Sri Lanka')
   INSERT INTO Country VALUES('BL', N'St. Barthélemy')
   INSERT INTO Country VALUES('SH', N'St. Helena')
   INSERT INTO Country VALUES('KN', N'St. Kitts and Nevis')
   INSERT INTO Country VALUES('LC', N'St. Lucia')
   INSERT INTO Country VALUES('MF', N'St. Martin')
   INSERT INTO Country VALUES('PM', N'St. Pierre and Miquelon')
   INSERT INTO Country VALUES('VC', N'St. Vincent and the Grenadines')
   INSERT INTO Country VALUES('SD', N'Sudan')
   INSERT INTO Country VALUES('SR', N'Suriname')
   INSERT INTO Country VALUES('SZ', N'Swaziland')
   INSERT INTO Country VALUES('SE', N'Sweden')
   INSERT INTO Country VALUES('CH', N'Switzerland')
   INSERT INTO Country VALUES('SY', N'Syria')
   INSERT INTO Country VALUES('TW', N'Taiwan')
   INSERT INTO Country VALUES('TJ', N'Tajikistan')
   INSERT INTO Country VALUES('TZ', N'Tanzania')
   INSERT INTO Country VALUES('TH', N'Thailand')
   INSERT INTO Country VALUES('TL', N'Timor-Leste')
   INSERT INTO Country VALUES('TG', N'Togo')
   INSERT INTO Country VALUES('TK', N'Tokelau')
   INSERT INTO Country VALUES('TO', N'Tonga')
   INSERT INTO Country VALUES('TT', N'Trinidad and Tobago')
   INSERT INTO Country VALUES('TN', N'Tunisia')
   INSERT INTO Country VALUES('TR', N'Turkey')
   INSERT INTO Country VALUES('TM', N'Turkmenistan')
   INSERT INTO Country VALUES('TC', N'Turks and Caicos Islands')
   INSERT INTO Country VALUES('TV', N'Tuvalu')
   INSERT INTO Country VALUES('UG', N'Uganda')
   INSERT INTO Country VALUES('UA', N'Ukraine')
   INSERT INTO Country VALUES('AE', N'United Arab Emirates')
   INSERT INTO Country VALUES('UK', N'United Kingdom')
   INSERT INTO Country VALUES('US', N'United States')
   INSERT INTO Country VALUES('UM', N'United States Minor Outlying Islands')
   INSERT INTO Country VALUES('UY', N'Uruguay')
   INSERT INTO Country VALUES('UZ', N'Uzbekistan')
   INSERT INTO Country VALUES('VU', N'Vanuatu')
   INSERT INTO Country VALUES('VA', N'Vatican City')
   INSERT INTO Country VALUES('VE', N'Venezuela')
   INSERT INTO Country VALUES('VN', N'Vietnam')
   INSERT INTO Country VALUES('VG', N'Virgin Islands, British')
   INSERT INTO Country VALUES('VI', N'Virgin Islands, U.S.')
   INSERT INTO Country VALUES('WF', N'Wallis and Futuna')
   INSERT INTO Country VALUES('YE', N'Yemen')
   INSERT INTO Country VALUES('ZM', N'Zambia')
   INSERT INTO Country VALUES('ZW', N'Zimbabwe')
   PRINT 'Data inserted to dbo.Country...'

   -- Insert admin record
   INSERT INTO Account(LoginUsername, LoginPassword) VALUES('e1', 'F9-E5-10-C7-B7-5D-A2-E0-2A-04-82-77-C0-29-6F-28')
   INSERT INTO Employee(EmployeeFirstName, EmployeeLastName, Gender, DateOfBirth, Address1,
                        City, PostCode, StateProvince, CountryCode, ContactNumber, Email,
                        RoleID, ModifiedDate, AccountID)
         VALUES('Admin', 'Admin', 0, '1/1/1990', 'a', 'Melbourne', '3000', 'Victoria',
                'AU', '0300000000', 'admin@admin.com', 1, CURRENT_TIMESTAMP, 1)
   PRINT 'Admin data inserted..'
END
GO


-- Create stored procedures -----------------------------------------------------------------------
CREATE PROCEDURE LoginVerification
   @username NVARCHAR(100), @password NVARCHAR(200)
AS
   SET NOCOUNT ON
   DECLARE @TITLE VARCHAR(20)
   DECLARE @RS TINYINT
   BEGIN
      IF SUBSTRING(@username, 1, 1) = 's'
         BEGIN
            IF (SELECT s.StudentID FROM Student s
                  INNER JOIN Account a ON s.AccountID = a.AccountID
                  WHERE a.LoginUsername = @username AND a.LoginPassword = @password) IS NOT NULL
               SET @RS = 4; --forward to student page
            ELSE
               SET @RS = 0; --Invalid username/password
         END
      ELSE IF SUBSTRING(@username, 1, 1) = 'e'
         BEGIN
            SET @TITLE = (SELECT rc.RoleTitle FROM Employee e
                  INNER JOIN Account a ON e.AccountID = a.AccountID
                  INNER JOIN RoleCategory rc ON e.RoleID = rc.RoleID
                  WHERE a.LoginUsername = @username AND a.LoginPassword = @password)

            IF @TITLE = 'Administrator'
               SET @RS = 1;
            ELSE IF @TITLE = 'Program Manager'
               SET @RS = 2;
            ELSE IF @TITLE = 'Staff'
               SET @RS = 3;
            ELSE
               SET @RS = 0; --Invalid username/password
         END
      ELSE
         SET @RS = 0; --Invalid username/password
      SELECT "Result" = @RS
   END
GO

CREATE PROCEDURE GetAllCountry
AS
   SELECT CountryCode, CountryTitle FROM Country ORDER BY CountryTitle
GO

CREATE PROCEDURE GetUserPersonalDetail
   @StudentUsername NVARCHAR(100)
AS
   SELECT s.StudentFirstName, s.StudentLastName, s.Gender, s.DateOfBirth,
          s.Address1, s.Address2, s.City, s.PostCode, s.StateProvince,
          s.ContactNumber, s.Email, c.CountryCode
      FROM Student s
         INNER JOIN Account a ON s.AccountID = a.AccountID
         INNER JOIN Country c ON s.CountryCode = c.CountryCode
         WHERE a.LoginUsername = @StudentUsername
GO

CREATE PROCEDURE UpdateStudentProfile
   @FirstName      NVARCHAR(150),
   @LastName       NVARCHAR(100),
   @Gender         BIT,
   @DOB            DATE,
   @Address1       NVARCHAR(100),
   @Address2       NVARCHAR(100),
   @City           NVARCHAR(100),
   @PostCode       NVARCHAR(20),
   @StateProvince  NVARCHAR(80),
   @CountryCode    NVARCHAR(5),
   @ContactNumber  NVARCHAR(15),
   @Email          NVARCHAR(255),
   @StudentID      INT
AS
   BEGIN
      UPDATE Student SET StudentFirstName = @FirstName, StudentLastName = @LastName,
         Gender = @Gender, DateOfBirth = @DOB, Address1 = @Address1,
         Address2 = @Address2, City = @City, PostCode = @PostCode,
         StateProvince = @StateProvince, CountryCode = @CountryCode,
         ContactNumber = @ContactNumber, Email = @Email
         WHERE StudentID = @StudentID
   END
GO

CREATE PROCEDURE UpdateUserPassword
   @StudentUsername NVARCHAR(100),
   @OldPassword NVARCHAR(255),
   @NewPassword NVARCHAR(255)
AS
   BEGIN
      UPDATE Account SET LoginPassword = @NewPassword
         WHERE LoginUsername = @StudentUsername AND LoginPassword = @OldPassword

      IF @@ROWCOUNT = 0
         RAISERROR('Incorrect old password', 16,1)
   END
GO

CREATE PROCEDURE GetAllCourse
AS
   SELECT * FROM Course
GO

CREATE PROCEDURE GetAllCourseOfProgram
   @STUDENTID INT
AS
   SELECT c.CourseID, c.CourseName, c.CourseCode, c.CourseDescription, c.StaffID,
         c.CreatedDate, c.ModifiedDate FROM Course c
         INNER JOIN ProgramCourse pc ON pc.CourseID = c.CourseID
         INNER JOIN Student s ON s.ProgramID = pc.ProgramID
         WHERE s.StudentID = @STUDENTID
GO

CREATE PROCEDURE GetStudentEnrollment
   @studentID INT
AS
   SELECT * FROM Enrollment WHERE StudentID = @studentID ORDER BY CourseID
GO

CREATE PROCEDURE EnrolStudentCourse
   @STUDENTID INT, @COURSEID INT
AS
   INSERT INTO Enrollment(StudentID, CourseID) VALUES(@STUDENTID, @COURSEID)
GO

CREATE PROCEDURE RemoveStudentCourse
   @STUDENTID INT, @COURSEID INT
AS
   delete Enrollment where StudentID=@STUDENTID and CourseID=@COURSEID
GO


-- PK's stored procedures -------------------------------------------------------------------------
CREATE PROCEDURE InsertDepartment
   @departmentName NVARCHAR(200)
AS
BEGIN
   INSERT Department(DepartmentName, ModifiedDate) VALUES (@departmentName, GETDATE())
END
GO

CREATE PROCEDURE InsertProgram
   @programName        NVARCHAR(200),
   @programDescription NVARCHAR(MAX)
AS
BEGIN
   INSERT Program(ProgramName, ProgramDescription, ModifiedDate) VALUES (@programName, @programDescription, GETDATE())
END
GO

CREATE PROCEDURE InsertAccount
   @loginUsername NVARCHAR(100),
   @loginPassword NVARCHAR(255)
AS
BEGIN
   INSERT Account(LoginUsername, LoginPassword) VALUES (@loginUsername, @loginPassword)
END
GO

CREATE PROCEDURE InsertEmployee
   @firstname      NVARCHAR(150),
   @lastname       NVARCHAR(100),
   @gender         BIT,
   @dateOfBirth    DATETIME,
   @address1       NVARCHAR(100),
   @address2       NVARCHAR(100),
   @city           NVARCHAR(100),
   @postCode       NVARCHAR(20),
   @stateProvince  NVARCHAR(80),
   @countryCode    NVARCHAR(5),
   @contactNumber  NVARCHAR(15),
   @emailAddress   NVARCHAR(255),
   @loginPassword  NVARCHAR(255),
   @roleId         TINYINT
AS
BEGIN
   DECLARE @AccountID INT
   DECLARE @loginUsername NVARCHAR(100)

   INSERT Employee(EmployeeFirstName, EmployeeLastName, Gender, DateOfBirth, Address1, Address2, City,PostCode,
                   StateProvince, CountryCode, ContactNumber, Email, ModifiedDate, RoleID)
      VALUES (@firstname, @lastname, @gender, @dateOfBirth, @address1, @address2, @city, @postCode,
              @stateProvince, @countryCode, @contactNumber, @emailAddress, GETDATE(), @roleId)

   SET @loginUsername = 'e' + CAST(SCOPE_IDENTITY() AS varchar(10))
   INSERT Account(LoginUsername, LoginPassword) VALUES (@loginUsername, @loginPassword)

   SET @AccountID = SCOPE_IDENTITY()
   UPDATE Employee SET AccountID = @AccountID
      WHERE EmployeeID = SUBSTRING(@loginUsername, 2, 10)
END
GO

CREATE PROCEDURE [dbo].[UpdateCourseWithStaffID]
   @courseId            INT,
   @courseName          NVARCHAR(200),
   @courseCode          NVARCHAR(60),
   @courseDescription   NVARCHAR(max),
   @staffId             INT
AS
BEGIN
   UPDATE Course SET CourseName = @courseName, CourseCode = @courseCode,
      CourseDescription = @courseDescription, ModifiedDate = GETDATE(),
      StaffID = @staffId
      WHERE CourseID = @courseId
END
GO

CREATE PROCEDURE [dbo].[UpdateProgramManager]
   @programId  int,
   @managerId  int
AS
BEGIN
   UPDATE Program SET ManagerID = @managerId WHERE ProgramID = @programId
END
GO

-- Edward's Stored Procedures ---------------------------------------------------------------------
CREATE PROCEDURE [dbo].[GetAllCourseOfStaff]
   @STAFFID INT
AS
   SELECT c.CourseID, c.CourseName, c.CourseCode, c.CourseDescription, c.StaffID,
         c.CreatedDate, c.ModifiedDate FROM Course c
         WHERE c.StaffID=@STAFFID
GO

CREATE PROCEDURE [dbo].[GetStudentEnrollmentByCourseID]
   @COURSEID INT
AS
   SELECT * FROM Enrollment WHERE CourseID=@COURSEID ORDER BY StudentID
GO

CREATE PROCEDURE [dbo].[InsertCourse]
   @courseName          nvarchar(200),
   @courseCode          nvarchar(60),
   @courseDescription   nvarchar(max),
   @programId           INT
AS
BEGIN
   DECLARE @courseId AS INT
   INSERT Course(CourseName, CourseCode, CourseDescription, CreatedDate, ModifiedDate)
   VALUES (@coursename, @coursecode, @coursedescription, GETDATE(), GETDATE())

   SET @courseId = SCOPE_IDENTITY()
   INSERT ProgramCourse(ProgramID, CourseID) VALUES (@programId, @courseId)
END
GO

CREATE PROCEDURE [dbo].[UpdateCourse]
   @courseid            int,
   @coursename          nvarchar(200),
   @coursecode          nvarchar(60),
   @coursedescription   nvarchar(max)
AS
BEGIN
   UPDATE Course SET CourseName = @coursename, CourseCode = @coursecode,
      CourseDescription = @coursedescription, ModifiedDate = GETDATE()
      WHERE CourseID = @courseid
END
GO

CREATE PROCEDURE [dbo].[InsertStudent]
   @loginPassword    NVARCHAR(255),
   @FirstName        NVARCHAR(150),
   @LastName         NVARCHAR(100),
   @Gender           BIT,
   @DOB              DATE,
   @Address1         NVARCHAR(100),
   @Address2         NVARCHAR(100),
   @City             NVARCHAR(100),
   @PostCode         NVARCHAR(20),
   @StateProvince    NVARCHAR(80),
   @CountryCode      NVARCHAR(5),
   @ContactNumber    NVARCHAR(15),
   @Email            NVARCHAR(255),
   @ProgramID        INT
AS
BEGIN
   DECLARE @AccountID int
   DECLARE @loginUsername NVARCHAR(100)

   INSERT Student(StudentFirstName, StudentLastName, Gender, DateOfBirth, Address1, Address2, City, PostCode,
                  StateProvince, CountryCode, ContactNumber, Email, AccountID, ProgramID, CreatedDate, ModifiedDate)
      VALUES (@FirstName, @LastName, @Gender, @DOB, @Address1, @Address2, @City, @PostCode, @StateProvince,
              @CountryCode, @ContactNumber, @Email, 1, @ProgramID, GETDATE(), GETDATE())

   SET @loginUsername = 's' + CAST(SCOPE_IDENTITY() AS varchar(10))
   INSERT Account(LoginUsername, LoginPassword) VALUES (@loginUsername, @loginPassword)

   SET @AccountID = SCOPE_IDENTITY()
   UPDATE Student SET AccountID = @AccountID
      WHERE StudentID = SUBSTRING(@loginUsername, 2, 10)
END
GO

CREATE PROCEDURE [dbo].[UpdateStudentMarks]
   @CourseID   int,
   @StudentID  int,
   @Marks      float
AS
BEGIN
   UPDATE StudentCourse SET Marks = @Marks
      WHERE CourseID = @CourseID and StudentID = @StudentID
END
GO

---------------------------------------------------------------------------------------------------

CREATE PROCEDURE GetLatestLoginUsername
AS
   SELECT TOP (1) loginusername FROM Account ORDER BY AccountID DESC
GO



PRINT 'Stored procedures created'
GO

-- Remove all objects
/*
SET NOCOUNT ON
USE AMS_DATABASE;
GO

-- Remove tables
IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ProgramCourse'))
   DROP TABLE dbo.ProgramCourse

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'EmployeeDepartment'))
   DROP TABLE dbo.EmployeeDepartment

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'StudentCourse'))
   DROP TABLE dbo.StudentCourse

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Department'))
   DROP TABLE dbo.Department

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Course'))
   DROP TABLE dbo.Course

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Student'))
   DROP TABLE dbo.Student

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Program'))
   DROP TABLE dbo.Program

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Employee'))
   DROP TABLE dbo.Employee

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Account'))
   DROP TABLE dbo.Account

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Country'))
   DROP TABLE dbo.Country

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'RoleCategory'))
   DROP TABLE dbo.RoleCategory
PRINT 'All tables are removed...'
GO

IF (OBJECT_ID('CourseMarksRule') IS NOT NULL)
   DROP RULE CourseMarksRule
PRINT 'Rule is removed...'
GO

IF (OBJECT_ID('DATETIMECREATED') IS NOT NULL)
   DROP DEFAULT DATETIMECREATED
PRINT 'Default is removed...'
GO

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME = 'Enrollment'))
   DROP VIEW Enrollment
GO

-- Drop all triggers ------------------------------------------------------
IF (OBJECT_ID('InsertDeleteStudentCourse') IS NOT NULL)
   DROP TRIGGER InsertDeleteStudentCourse

IF (OBJECT_ID('UpdateStudentCourseScore') IS NOT NULL)
   DROP TRIGGER UpdateStudentCourseScore

IF (OBJECT_ID('AvgScorePerCourse') IS NOT NULL)
   DROP TRIGGER AvgScorePerCourse
PRINT 'Triggers are removed...'
GO

-- Drop all Stored Procedures ---------------------------------------------
IF (OBJECT_ID('LoginVerification') IS NOT NULL)
   DROP PROCEDURE LoginVerification

IF (OBJECT_ID('GetAllCountry') IS NOT NULL)
   DROP PROCEDURE GetAllCountry

IF (OBJECT_ID('GetUserPersonalDetail') IS NOT NULL)
   DROP PROCEDURE GetUserPersonalDetail

IF (OBJECT_ID('UpdateStudentProfile') IS NOT NULL)
   DROP PROCEDURE UpdateStudentProfile

IF (OBJECT_ID('UpdateUserPassword') IS NOT NULL)
   DROP PROCEDURE UpdateUserPassword

IF (OBJECT_ID('GetAllCourse') IS NOT NULL)
   DROP PROCEDURE GetAllCourse

IF (OBJECT_ID('GetStudentEnrollment') IS NOT NULL)
   DROP PROCEDURE GetStudentEnrollment

IF (OBJECT_ID('EnrolStudentCourse') IS NOT NULL)
   DROP PROCEDURE EnrolStudentCourse

IF (OBJECT_ID('RemoveStudentCourse') IS NOT NULL)
   DROP PROCEDURE RemoveStudentCourse

IF (OBJECT_ID('GetAllCourseOfProgram') IS NOT NULL)
   DROP PROCEDURE GetAllCourseOfProgram

-- PK's Stored Procedures -------------------------------------------------------------------------
IF (OBJECT_ID('InsertDepartment') IS NOT NULL)
   DROP PROCEDURE InsertDepartment

IF (OBJECT_ID('InsertProgram') IS NOT NULL)
   DROP PROCEDURE InsertProgram

IF (OBJECT_ID('InsertAccount') IS NOT NULL)
   DROP PROCEDURE InsertAccount

IF (OBJECT_ID('InsertEmployee') IS NOT NULL)
   DROP PROCEDURE InsertEmployee

IF (OBJECT_ID('UpdateCourseWithStaffID') IS NOT NULL)
   DROP PROCEDURE UpdateCourseWithStaffID

IF (OBJECT_ID('UpdateProgramManager') IS NOT NULL)
   DROP PROCEDURE UpdateProgramManager

-- Edward's Stored Procedures ---------------------------------------------------------------------
IF (OBJECT_ID('InsertStudent') IS NOT NULL)
   DROP PROCEDURE InsertStudent

IF (OBJECT_ID('UpdateCourse') IS NOT NULL)
   DROP PROCEDURE UpdateCourse

IF (OBJECT_ID('InsertCourse') IS NOT NULL)
   DROP PROCEDURE InsertCourse

IF (OBJECT_ID('GetStudentEnrollmentByCourseID') IS NOT NULL)
   DROP PROCEDURE GetStudentEnrollmentByCourseID

IF (OBJECT_ID('GetAllCourseOfStaff') IS NOT NULL)
   DROP PROCEDURE GetAllCourseOfStaff

IF (OBJECT_ID('UpdateStudentMarks') IS NOT NULL)
   DROP PROCEDURE UpdateStudentMarks

IF (OBJECT_ID('GetLatestLoginUsername') IS NOT NULL)
   DROP PROCEDURE GetLatestLoginUsername

PRINT 'All stored procedures are removed...'
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LOGIN')
    DROP INDEX IX_LOGIN ON Account;

IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_COURSE')
    DROP INDEX IX_COURSE ON Course;
PRINT 'Indexes are removed...'
GO

-- Remove entire database
USE master
GO
IF (EXISTS (SELECT * FROM sys.databases WHERE name = 'AMS_DATABASE'))
BEGIN
   DROP DATABASE AMS_DATABASE;
   PRINT 'Database AMS_DATABASE is deleted.'
END
GO
*/
