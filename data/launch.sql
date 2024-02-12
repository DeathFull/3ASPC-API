IF NOT EXISTS (SELECT name
               FROM sys.databases
               WHERE name = 'ibay')
CREATE DATABASE ibay;
go

USE ibay;

CREATE TABLE Users
(
    Id       INT PRIMARY KEY IDENTITY,
    Email    NVARCHAR(320) NOT NULL,
    Pseudo   NVARCHAR(50)  NOT NULL,
    Password NVARCHAR(200)  NOT NULL,
    Role     NVARCHAR(50)  NOT NULL,
    CONSTRAINT UC_User_Email UNIQUE (Email),
    CONSTRAINT UC_User_Pseudo UNIQUE (Pseudo)
);

CREATE TABLE Products
(
    Id        INT PRIMARY KEY IDENTITY,
    Name      NVARCHAR(50)   NOT NULL,
    Image     NVARCHAR(255)  NOT NULL,
    Price     DECIMAL(18, 2) NOT NULL,
    Available BIT            NOT NULL,
    AddedTime DATETIME       NOT NULL,
    SellerId  INT            NOT NULL,
    FOREIGN KEY (SellerId) REFERENCES Users(ID)
);

CREATE TABLE Carts
(
    Id     INT PRIMARY KEY IDENTITY,
    UserId INT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users (Id)
);

CREATE TABLE CartItems
(
    Id        INT PRIMARY KEY IDENTITY,
    CartId    INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity  INT NOT NULL,
    FOREIGN KEY (ProductId) REFERENCES Products (Id),
    FOREIGN KEY (CartId) REFERENCES Carts (Id)
);

