# PlantNest Project

## Overview

PlantNest is a web application designed for managing a variety of plant-related tasks. It features functionality for managing users, plants, accessories, orders, and more. The system includes both an administrative panel for administrative tasks and user login for regular access.

## Features

- **Admin Dashboard**: Provides administrative control to manage users, plants, accessories, and orders.
- **User Management**: Allows users to register, log in, and manage their profiles.
- **Plant Management**: Enables adding, updating, and viewing plant details.
- **Order Management**: Allows users to place orders and track their status.
- **Feedback and Reviews**: Users can provide feedback and rate plants and accessories.

## Installation

1. **Clone the Repository**

   ```bash
   git clone https://github.com/MuhammadArslan6464/plantnest.git

   Set Up the Database

Create a new SQL Server database named PlantNest.
Run the following SQL scripts to create tables and insert initial data:

create table AdminLogin(
AdminID int primary key identity (1,1),
AdName nvarchar (50),
Password nvarchar (50)
)

INSERT INTO AdminLogin (AdName, Password)
VALUES ('Admin', '123456');

select * From AdminLogin

CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    ContactNumber NVARCHAR(15) NOT NULL,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL
);

select * from Users

CREATE TABLE Plants (
    PlantID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Species NVARCHAR(100),
    Price DECIMAL(10, 2) NOT NULL,
    Discount DECIMAL(5, 2),
    Description TEXT,
    Category NVARCHAR(50),
    ImageURL NVARCHAR(255),
	StockQuantity INT NOT NULL
);

select * From Plants

CREATE TABLE Accessories (
    AccessoryID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Purpose NVARCHAR(255),
    Price DECIMAL(10, 2) NOT NULL,
    ImageURL NVARCHAR(255),
    StockQuantity INT NOT NULL
);

select * From Accessories


CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    OrderDate DATETIME DEFAULT GETDATE(),
	TotalAmount DECIMAL(10, 2),
    Status NVARCHAR(50)
);
select * From Orders

CREATE TABLE CartItems (
    CartItemId INT IDENTITY(1,1) PRIMARY KEY,  -- Auto-incrementing primary key
    PlantID INT NOT NULL,                      -- Foreign key to Plants table
    Quantity INT NOT NULL,                     -- Quantity of plants
    UserId NVARCHAR(128),                      -- User ID (String type, adjust length based on your needs)
    CONSTRAINT FK_CartItems_Plants FOREIGN KEY (PlantID) REFERENCES Plants(PlantID)  -- Foreign key constraint
);

select * from CartItems


CREATE TABLE Reviews (
    ReviewID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    PlantID INT FOREIGN KEY REFERENCES Plants(PlantID),
    AccessoryID INT FOREIGN KEY REFERENCES Accessories(AccessoryID),
    Rating INT CHECK (Rating >= 1 AND Rating <= 5),
    Comment TEXT,
    ReviewDate DATETIME DEFAULT GETDATE()
);

select * From Reviews

CREATE TABLE Feedback (
    FeedbackID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    Message TEXT,
    FeedbackDate DATETIME DEFAULT GETDATE()
);

select * From Feedback

CREATE TABLE Contact (
    ContactID INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(20),
    Address NVARCHAR(255)
);

Select * From Contact


Usage
Admin Login
Access the Admin Login Page

Navigate to /AdminPanel/Login in your browser.

Login Credentials
Username: Admin
Password: 123456


Features Accessible:
Manage users, plants, accessories, and orders from the admin dashboard.


User Login
Access the User Login Page

Navigate to /Home/Index in your browser.

Login Credentials

Use the credentials you registered with during user registration.
Features Accessible

View and manage your personal profile.
Browse and order plants and accessories.
Provide feedback and write reviews.


Database Schema
Here’s a summary of the key tables in the database:

AdminLogin: Contains administrative login credentials.
Users: Stores user information and login details.
Plants: Details about plants available for purchase.
Accessories: Information about plant-related accessories.
Orders: Records of orders placed by users.
CartItems: Items added to user carts.
Reviews: Reviews provided by users for plants and accessories.
Feedback: Feedback messages from users.
Contact: Contact information for the company.



### Notes

- **Database Schema**: Ensure that the SQL scripts match your actual database schema.
- **Usage**: Customize the URLs and credentials as needed for your application.
- **Contributing**: Adjust the contributing section based on your project’s contribution guidelines.

This README template provides a thorough overview of your project, installation instructions, usage details, and contribution guidelines. Modify it as necessary to fit the specifics of your project.
