-- Created by eric on 3/6/26 - 12:41:22
INSERT INTO item(name, color, pricing_model, created_by)
SELECT v.name, v.color, v.pricing_model, u.id
FROM host_user u
         CROSS JOIN(VALUES ('Coca Cola', null, 'size'::pricing_model),
                           ('Pepsi', null, 'base'::pricing_model),
                           ('Sprite', decode('FF000000', 'hex'),
                            'base'::pricing_model)) AS v(name, color, pricing_model);

INSERT INTO item_base_price(price, denomination, item_id)
VALUES (1.99, 'usd', 2),
       (1.89, 'usd', 3);

INSERT INTO item_size_price(size, price, denomination, item_id)
VALUES ('Small', 1.49, 'usd', 1),
       ('Medium', 1.79, 'usd', 1),
       ('Large', 1.99, 'usd', 1)
