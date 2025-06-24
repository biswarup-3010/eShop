-- 1) Create Database
CREATE DATABASE IF NOT EXISTS eShopDB;
USE eShopDB;

-- 2) Roles Table
CREATE TABLE Roles (
    RoleID INT AUTO_INCREMENT PRIMARY KEY,
    RoleName VARCHAR(50) UNIQUE NOT NULL
);

-- 3) Users Table
CREATE TABLE Users (
    UserID INT AUTO_INCREMENT PRIMARY KEY,
    FullName VARCHAR(100),
    Email VARCHAR(100) UNIQUE,
    PasswordHash VARCHAR(255),
    Phone VARCHAR(20),
    Address TEXT,
    RoleID INT,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID)
);

-- 4) Categories Table
CREATE TABLE Categories (
    CategoryID INT AUTO_INCREMENT PRIMARY KEY,
    CategoryName VARCHAR(100) NOT NULL
);

-- 5) Products Table
CREATE TABLE Products (
    ProductID INT AUTO_INCREMENT PRIMARY KEY,
    ProductName VARCHAR(100) NOT NULL,
    Description TEXT,
    Price DECIMAL(10,2) NOT NULL,
    Stock INT DEFAULT 0,
    CategoryID INT,
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);

-- 6) ProductImages Table (NEW: supports multiple images per product)
CREATE TABLE ProductImages (
    ImageID INT AUTO_INCREMENT PRIMARY KEY,
    ProductID INT,
    ImageURL VARCHAR(255) NOT NULL,
    IsPrimary BOOLEAN DEFAULT FALSE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE
);

-- 7) Orders Table
CREATE TABLE Orders (
    OrderID INT AUTO_INCREMENT PRIMARY KEY,
    UserID INT,
    OrderDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    Status ENUM('Pending', 'Shipped', 'Delivered', 'Cancelled') DEFAULT 'Pending',
    TotalAmount DECIMAL(10,2),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- 8) OrderDetails Table
CREATE TABLE OrderDetails (
    OrderDetailID INT AUTO_INCREMENT PRIMARY KEY,
    OrderID INT,
    ProductID INT,
    Quantity INT,
    PriceAtPurchase DECIMAL(10,2),
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

-- 9) Reviews Table
CREATE TABLE Reviews (
    ReviewID INT AUTO_INCREMENT PRIMARY KEY,
    UserID INT,
    ProductID INT,
    Rating INT CHECK (Rating BETWEEN 1 AND 5),
    Comment TEXT,
    ReviewDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

-- 10) Cart Table
CREATE TABLE Cart (
    CartID INT AUTO_INCREMENT PRIMARY KEY,
    UserID INT,
    ProductID INT,
    Quantity INT DEFAULT 1,
    AddedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

-- 11) Wishlist Table
CREATE TABLE Wishlist (
    WishlistID INT AUTO_INCREMENT PRIMARY KEY,
    UserID INT,
    ProductID INT,
    AddedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

-- 12) UserInterest Table
CREATE TABLE UserInterest (
    InterestID INT AUTO_INCREMENT PRIMARY KEY,
    UserID INT,
    CategoryID INT,
    InterestedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);

-- 13) Purchase Table
CREATE TABLE Purchase (
    PurchaseID INT AUTO_INCREMENT PRIMARY KEY,
    OrderID INT,
    PurchaseDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    PaymentMethod VARCHAR(50),
    PaymentStatus ENUM('Success', 'Failed', 'Pending') DEFAULT 'Pending',
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID)
);




-- stored procedures
DELIMITER $$

CREATE PROCEDURE UpdateProduct(
    IN p_ProductId INT,
    IN p_ProductName VARCHAR(100),
    IN p_Description TEXT,
    IN p_Price DECIMAL(10,2),
    IN p_Stock INT,
    IN p_CategoryId INT
)
BEGIN
    DECLARE product_exists INT;

    SELECT COUNT(*) INTO product_exists
    FROM Products
    WHERE ProductID = p_ProductId;

    IF product_exists = 1 THEN
        UPDATE Products
        SET
            ProductName = p_ProductName,
            Description = p_Description,
            Price = p_Price,
            Stock = p_Stock,
            CategoryID = p_CategoryId
        WHERE ProductID = p_ProductId;

        SELECT 'Success' AS Status;
    ELSE
        SELECT 'Product Not Found' AS Status;
    END IF;
END $$

DELIMITER ;


DELIMITER $$

CREATE PROCEDURE UserLogin(
    IN p_Email VARCHAR(100),
    IN p_Password VARCHAR(255)
)
BEGIN
    DECLARE user_exists INT;

    -- Check if user with email and password exists
    SELECT COUNT(*) INTO user_exists
    FROM Users
    WHERE Email = p_Email AND PasswordHash = p_Password;

    IF user_exists = 1 THEN
        -- Return user details and status
        SELECT 
            'Success' AS Status,
            UserID,
            FullName,
            Email,
            Phone,
            Address,
            RoleID,
            CreatedAt
        FROM Users
        WHERE Email = p_Email AND PasswordHash = p_Password;
    ELSE
        -- Return only the status
        SELECT 'Invalid Credentials' AS Status;
    END IF;
END $$

DELIMITER ;


DELIMITER $$

CREATE PROCEDURE ForgotPassword(
    IN p_Email VARCHAR(100),
    IN p_NewPassword VARCHAR(255),
    OUT p_Status VARCHAR(50)
)
BEGIN
    DECLARE user_exists INT;

    -- Check if user exists
    SELECT COUNT(*) INTO user_exists
    FROM Users
    WHERE Email = p_Email;

    IF user_exists = 1 THEN
        -- Update the password
        UPDATE Users
        SET PasswordHash = p_NewPassword
        WHERE Email = p_Email;

        SET p_Status = 'Success';
    ELSE
        SET p_Status = 'User Not Found';
    END IF;
END $$

DELIMITER ;


DELIMITER $$

CREATE PROCEDURE UpdateUser (
    IN p_UserId INT,
    IN p_FullName VARCHAR(100),
    IN p_Email VARCHAR(100),
    IN p_Phone VARCHAR(20),
    IN p_Address TEXT,
    IN p_RoleId INT,
    IN p_CreatedAt DATETIME
)
BEGIN
    DECLARE user_exists INT;

    SELECT COUNT(*) INTO user_exists FROM Users WHERE UserID = p_UserId;

    IF user_exists = 1 THEN
        UPDATE Users
        SET
            FullName = p_FullName,
            Email = p_Email,
            Phone = p_Phone,
            Address = p_Address,
            RoleID = p_RoleId,
            CreatedAt = p_CreatedAt
        WHERE UserID = p_UserId;

        SELECT 'Success' AS Status;
    ELSE
        SELECT 'User Not Found' AS Status;
    END IF;
END $$

DELIMITER ;











-- 5) Demo Data Inserts

-- Roles
INSERT INTO Roles (RoleName) VALUES 
('Customer'), 
('Admin'), 
('Vendor');

-- Users
INSERT INTO Users (FullName, Email, PasswordHash, Phone, Address, RoleID) VALUES
('Alice Smith','alice@example.com','pass123','1234567890','123 Apple St',1),
('Bob Johnson','bob@example.com','pass456','1234567891','456 Banana Ave',2),
('Charlie Brown','charlie@example.com','pass789','1234567892','789 Cherry Rd',3),
('Diana Ross','diana@example.com','pass321','1234567893','321 Grape Ln',1),
('Evan Lee','evan@example.com','pass654','1234567894','654 Orange Blvd',1),
('Fiona Wu','fiona@example.com','pass987','1234567895','987 Pear Ct',1),
('George King','george@example.com','passabc','1234567896','111 Mango Dr',1),
('Hannah Green','hannah@example.com','passdef','1234567897','222 Berry Ln',1),
('Ian Gray','ian@example.com','passghi','1234567898','333 Kiwi St',1),
('Julia Snow','julia@example.com','passjkl','1234567899','444 Pine Ave',1);

-- Categories
INSERT INTO Categories (CategoryName) VALUES
('Electronics'), 
('Clothing'), 
('Books'), 
('Home & Kitchen'), 
('Sports');

-- Products
INSERT INTO Products (ProductName, Description, Price, Stock, CategoryID) VALUES
('Smartphone','Latest Android smartphone',299.99,100,1),
('Laptop','High-performance laptop',899.99,50,1),
('T-shirt','Cotton T-shirt',19.99,200,2),
('Jeans','Slim fit jeans',39.99,150,2),
('Cookbook','Healthy recipes',24.99,80,3),
('Novel','Bestselling fiction novel',14.99,100,3),
('Blender','High-speed blender',49.99,60,4),
('Vacuum Cleaner','Powerful vacuum',129.99,40,4),
('Football','Standard size football',29.99,70,5),
('Yoga Mat','Non-slip yoga mat',25.99,90,5);

-- Product Images (each product has multiple images)
INSERT INTO ProductImages (ProductID, ImageURL, IsPrimary) VALUES
(1, '/images/products/1_1.jpg', TRUE),
(1, '/images/products/1_2.jpg', FALSE),
(2, '/images/products/2_1.jpg', TRUE),
(2, '/images/products/2_2.jpg', FALSE),
(3, '/images/products/3_1.jpg', TRUE),
(3, '/images/products/3_2.jpg', FALSE),
(4, '/images/products/4_1.jpg', TRUE),
(5, '/images/products/5_1.jpg', TRUE),
(5, '/images/products/5_2.jpg', FALSE),
(6, '/images/products/6_1.jpg', TRUE),
(7, '/images/products/7_1.jpg', TRUE),
(8, '/images/products/8_1.jpg', TRUE),
(9, '/images/products/9_1.jpg', TRUE),
(10, '/images/products/10_1.jpg', TRUE);

-- Orders
INSERT INTO Orders (UserID, OrderDate, Status, TotalAmount) VALUES
(1,NOW(),'Pending',319.98),
(4,NOW(),'Delivered',19.99),
(5,NOW(),'Shipped',64.98),
(6,NOW(),'Pending',899.99),
(7,NOW(),'Delivered',49.99),
(1,NOW(),'Cancelled',24.99),
(8,NOW(),'Delivered',44.98),
(9,NOW(),'Pending',39.99),
(10,NOW(),'Shipped',25.99),
(3,NOW(),'Pending',129.99);

-- OrderDetails
INSERT INTO OrderDetails (OrderID, ProductID, Quantity, PriceAtPurchase) VALUES
(1,1,1,299.99),(1,3,1,19.99),(2,3,1,19.99),
(3,5,1,24.99),(3,10,1,39.99),(4,2,1,899.99),
(5,7,1,49.99),(6,5,1,24.99),(7,4,1,24.99),
(7,3,1,19.99),(8,4,1,39.99),(9,10,1,25.99),(10,8,1,129.99);

-- Reviews
INSERT INTO Reviews (UserID, ProductID, Rating, Comment) VALUES
(1,1,5,'Great phone!'),
(4,3,4,'Nice fit and quality'),
(5,5,5,'Love the recipes'),
(6,2,4,'Good performance'),
(7,7,3,'Decent for the price'),
(1,5,4,'Healthy ideas'),
(8,4,5,'Perfect jeans'),
(9,10,4,'Comfortable mat'),
(10,8,3,'Works well'),
(3,9,5,'Great football for practice');

-- Cart
INSERT INTO Cart (UserID, ProductID, Quantity) VALUES
(1, 1, 2),
(2, 3, 1),
(3, 6, 1),
(4, 4, 2),
(5, 7, 1),
(6, 2, 1),
(7, 10, 1),
(8, 9, 1),
(9, 8, 1),
(10, 5, 2);

-- Wishlist
INSERT INTO Wishlist (UserID, ProductID) VALUES
(1, 2),
(2, 5),
(3, 8),
(4, 1),
(5, 10),
(6, 4),
(7, 3),
(8, 6),
(9, 9),
(10, 7);

-- User Interest
INSERT INTO UserInterest (UserID, CategoryID) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5),
(6, 1),
(7, 2),
(8, 3),
(9, 4),
(10, 5);

-- Purchase
INSERT INTO Purchase (OrderID, PaymentMethod, PaymentStatus) VALUES
(2, 'Credit Card', 'Success'),
(3, 'UPI', 'Success'),
(4, 'Cash on Delivery', 'Pending'),
(5, 'Debit Card', 'Success'),
(6, 'Net Banking', 'Failed'),
(7, 'UPI', 'Success'),
(8, 'Credit Card', 'Success'),
(9, 'Cash on Delivery', 'Pending'),
(10, 'Net Banking', 'Pending'),
(1, 'Debit Card', 'Success');





select * from Users;
select * from products;
select * from Cart;
select * from userinterest;
select * from categories;
select * from orders;
select * from productImages;
