-- Insert 15 publishers with their addresses
INSERT INTO Address (Street, Number, City, PostalCode, Country)
VALUES
    ('Main St', '123', 'New York', '10001', 'United States of America'),
    ('Elm St','456', 'Los Angeles', '90001', 'United States of America'),
    ('Oak Ave', '789', 'Chicago', '60601', 'United States of America'),
    ('Maple Rd', '234', 'London', 'SW1A 1AA', 'United Kingdom of Great Britain and Northern Ireland'),
    ('Pine Lane', '567', 'Tokyo', '100-0001', 'Japan'),
    ('Cedar Street', '890', 'Paris', '75001', 'France'),
    ('Birch Blvd', '345', 'Berlin', '10115', 'Germany'),
    ('Willow Way', '678', 'Sydney', '2000', 'Australia'),
    ('Oakwood Drive', '901', 'Toronto', 'M5H 1W7', 'Canada'),
    ('Magnolia Ave', '123', 'Rome', '00100', 'Italy'),
    ('Rose Street', '456', 'Seoul', '04550', 'South Korea'),
    ('Jasmine Lane', '789', 'Sao Paulo', '01000-000', 'Brazil'),
    ('Poplar Rd', '234', 'Moscow', '101000', 'Russia'),
    ('Cedar Ave', '567', 'Mexico City', '06000', 'Mexico'),
    ('Willow Lane', '890', 'Cape Town', '8001', 'South Africa'),
	('Knez Mihailova St', '12', 'Belgrade', '11000', 'Serbia'),
    ('Nikola Tesla St', '56', 'Novi Sad', '21000', 'Serbia')
;

INSERT INTO Publishers(Name, AddressId, Established)
VALUES
    ('Penguin Random House', 2, '1927-01-01'),
    ('HarperCollins', 3, '1817-03-10'),
    ('Simon & Schuster', 4, '1924-05-15'),
    ('Bloomsbury Publishing', 5, '1986-12-01'),
    ('Kodansha', 6, '1909-03-01'),
    ('Hachette Livre', 7, '1826-02-20'),
    ('Springer Nature', 8, '1842-07-18'),
    ('Allen & Unwin', 9, '1914-01-01'),
    ('McClelland & Stewart', 10, '1906-04-01'),
    ('Mondadori', 11, '1907-01-01'),
    ('Korea Publishers Association', 12, '1964-09-01'),
    ('Editora Abril', 13, '1950-04-10'),
    ('Eksmo', 14, '1991-03-30'),
    ('Grupo Planeta', 15, '1949-05-01'),
    ('NB Publishers', 16, '1971-08-20'),
	('Laguna', 17, '2000-01-01'),
    ('Vulkan', 18, '1995-05-10')
;
