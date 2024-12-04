/*DROP DATABASE IF EXISTS CSDL_DuLich;
CREATE DATABASE CSDL_DuLich;*/
/* Khởi tạo cơ sở dữ liệu */
USE CSDL_DuLich;

/* Tạo bảng Destinations */
CREATE TABLE Destinations (
    destination_id INT PRIMARY KEY IDENTITY(1,1),
    destination_name VARCHAR(255) NOT NULL,
    description TEXT
);

/* Tạo bảng Employees */
CREATE TABLE Employees (
    employee_id INT PRIMARY KEY IDENTITY(1,1),
    full_name VARCHAR(255) NOT NULL,
    position VARCHAR(100),
    hire_date DATE,
    phone_number VARCHAR(20)
);

/* Tạo bảng TourGuides */
CREATE TABLE TourGuides (
    guide_id INT PRIMARY KEY IDENTITY(1,1),
    employee_id INT NOT NULL,
    experience_years INT,
    language_spoken VARCHAR(100),
    FOREIGN KEY (employee_id) REFERENCES Employees(employee_id)
);

/* Tạo bảng Tours với destination_id */
CREATE TABLE Tours (
    tour_id INT PRIMARY KEY IDENTITY(1,1),
    tour_name VARCHAR(255) NOT NULL,
    description TEXT,
    price DECIMAL(10, 2) NOT NULL,
    start_day DATE,
    end_day DATE,
    destination_id INT NOT NULL,
	guide_id INT NOT NULL,
    FOREIGN KEY (destination_id) REFERENCES Destinations(destination_id),
	FOREIGN KEY (guide_id) REFERENCES TourGuides(guide_id)
);

/* Tạo bảng Customers */
CREATE TABLE Customers (
    customer_id INT PRIMARY KEY IDENTITY(1,1),
    full_name VARCHAR(255) NOT NULL,
    email VARCHAR(255),
    phone_number VARCHAR(20),
    address TEXT
);

/* Tạo bảng Bookings */
CREATE TABLE Bookings (
    booking_id INT PRIMARY KEY IDENTITY(1,1),
    customer_id INT NOT NULL,
    tour_id INT NOT NULL,
    booking_date DATE,
    status VARCHAR(50),
    total_price DECIMAL(10, 2),
    FOREIGN KEY (customer_id) REFERENCES Customers(customer_id),
    FOREIGN KEY (tour_id) REFERENCES Tours(tour_id)
);

/* Tạo bảng Payments */
CREATE TABLE Payments (
    payment_id INT PRIMARY KEY IDENTITY(1,1),
    booking_id INT NOT NULL,
    payment_date DATE,
    amount DECIMAL(10, 2),
    payment_method VARCHAR(50),
    FOREIGN KEY (booking_id) REFERENCES Bookings(booking_id)
);

/* Tạo bảng Feedbacks */
CREATE TABLE Feedbacks (
    feedback_id INT PRIMARY KEY IDENTITY(1,1),
    customer_id INT NOT NULL,
    tour_id INT NOT NULL,
    rating INT,
    comments TEXT,
    FOREIGN KEY (customer_id) REFERENCES Customers(customer_id),
    FOREIGN KEY (tour_id) REFERENCES Tours(tour_id)
);

/* Tạo bảng Accounts */
CREATE TABLE Accounts (
    username VARCHAR(255) PRIMARY KEY,
    password VARCHAR(255) NOT NULL,
    role VARCHAR(50) NOT NULL,
    customer_id INT NULL,
    employee_id INT NULL,
    FOREIGN KEY (customer_id) REFERENCES Customers(customer_id),
    FOREIGN KEY (employee_id) REFERENCES Employees(employee_id),
    CONSTRAINT CHK_Account_OneForeignKeyOnly CHECK (
        (customer_id IS NOT NULL AND employee_id IS NULL) OR 
        (customer_id IS NULL AND employee_id IS NOT NULL)
    )
);

-- Dữ liệu mẫu cho Destinations
INSERT INTO Destinations (destination_name, description) VALUES
('Hanoi', 'Capital of Vietnam with rich history and culture'),
('Halong Bay', 'Beautiful natural wonder with limestone islands'),
('Hue', 'Historical city with ancient architecture');

-- Dữ liệu mẫu cho Employees
INSERT INTO Employees (full_name, position, hire_date, phone_number) VALUES
('Alice Johnson', 'Tour Guide', '2020-01-10', '1234567890'),
('Bob Williams', 'Tour Guide', '2019-06-15', '0987654321'),
('Charlie Davis', 'Manager', '2018-04-20', '1122334455');

-- Dữ liệu mẫu cho TourGuides
INSERT INTO TourGuides (employee_id, experience_years, language_spoken) VALUES
(1, 5, 'English'),
(2, 8, 'English, French');

-- Dữ liệu mẫu cho Tours
INSERT INTO Tours (tour_name, description, price, start_day, end_day, destination_id, guide_id) VALUES
('Northern Adventure', '7-day tour of Northern Vietnam', 500.00, '2024-12-10', '2024-12-17', 1, 1),
('Beach Paradise', 'Relaxing tour at the famous beaches', 700.00, '2024-12-15', '2024-12-20', 2, 2),
('Cultural Exploration', 'Explore the history and culture of central Vietnam', 350.00, '2024-12-20', '2024-12-25', 3, 1);

-- Dữ liệu mẫu cho Customers
INSERT INTO Customers (full_name, email, phone_number, address) VALUES
('John Doe', 'john@example.com', '123456789', '123 Main St, Hanoi, Vietnam'),
('Jane Smith', 'jane@example.com', '987654321', '456 Elm St, Da Nang, Vietnam'),
('Robert Brown', 'robert@example.com', '555888999', '789 Oak St, Ho Chi Minh City, Vietnam');

-- Dữ liệu mẫu cho Bookings
INSERT INTO Bookings (customer_id, tour_id, booking_date, status, total_price) VALUES
(1, 1, '2024-11-01', 'Confirmed', 500.00),
(2, 2, '2024-11-02', 'Pending', 700.00),
(3, 3, '2024-11-03', 'Confirmed', 350.00);

-- Dữ liệu mẫu cho Payments
INSERT INTO Payments (booking_id, payment_date, amount, payment_method) VALUES
(1, '2024-11-05', 500.00, 'Credit Card'),
(2, '2024-11-06', 700.00, 'Bank Transfer'),
(3, '2024-11-07', 350.00, 'PayPal');

-- Dữ liệu mẫu cho Feedbacks
INSERT INTO Feedbacks (customer_id, tour_id, rating, comments) VALUES
(1, 1, 5, 'Amazing experience, highly recommend!'),
(2, 2, 4, 'Beautiful scenery, but the weather was a bit too hot'),
(3, 3, 5, 'Loved the cultural tour, very informative.');

-- Dữ liệu mẫu cho Accounts
INSERT INTO Accounts (username, password, role, customer_id, employee_id) VALUES
('admin', 'adminpassword', 'Admin', NULL, 3), -- Admin
('johndoe', 'password123', 'Customer', 1, NULL), -- Tài khoản cho John Doe
('janesmith', 'password456', 'Customer', 2, NULL), -- Tài khoản cho Jane Smith
('robertbrown', 'password789', 'Customer', 3, NULL), -- Tài khoản cho Robert Brown
('alicejohnson', 'passwordalice', 'Employee', NULL, 1), -- Tài khoản cho Alice Johnson (Tour Guide)
('bobwilliams', 'passwordbob', 'Employee', NULL, 2); -- Tài khoản cho Bob Williams (Tour Guide)




