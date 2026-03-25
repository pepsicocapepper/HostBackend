-- Created by eric on 3/5/26 - 12:20:47
INSERT INTO menu(name, menu_id, display_order, color)
VALUES ('Tacos', NULL, 1, decode('FFFF0000', 'hex')),
       ('Cakes', NULL, 2, null),
       ('Spicy Tacos', 1, 1, decode('FFFF0000', 'hex')),
       ('Guacamole Tacos', 1, 2, null),
       ('Vanilla Cakes', 2, 1, null),
       ('Chocolate Cakes', 2, 2, null)
