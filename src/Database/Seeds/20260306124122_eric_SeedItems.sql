-- Created by eric on 3/6/26 - 12:41:22
INSERT INTO item(name, created_by)
SELECT v.name, u.id
FROM host_user u
         CROSS JOIN(VALUES ('Coca Cola'),
                           ('Pepsi'),
                           ('Sprite')) AS v(name);

INSERT INTO item_price(price, denomination, item_id)
VALUES (2.99, 'usd', 1),
       (1.99, 'usd', 2),
       (1.89, 'usd', 3);