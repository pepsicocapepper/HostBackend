-- Created by eric on 3/6/26 - 12:41:22
INSERT INTO item(name, price, created_by)
SELECT v.name, v.price, u.id
FROM host_user u
         CROSS JOIN(VALUES ('Coca Cola', 1.99),
                           ('Pepsi', 1.89),
                           ('Sprite', 1.50)) AS v(name, price);
