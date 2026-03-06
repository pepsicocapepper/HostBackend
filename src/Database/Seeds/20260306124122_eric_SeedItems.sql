-- Created by eric on 3/6/26 - 12:41:22
INSERT INTO item(name, price, created_by)
SELECT 'Coca Cola', 1.99, id
FROM host_user;