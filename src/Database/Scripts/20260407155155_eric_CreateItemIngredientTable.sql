-- Created by eric on 4/7/26 - 15:51:55
CREATE TABLE item_ingredient
(
    item_id       INT            NOT NULL,
    ingredient_id INT            NOT NULL,
    quantity      DECIMAL(10, 3) NOT NULL,
    PRIMARY KEY (item_id, ingredient_id),
    FOREIGN KEY (item_id) REFERENCES item (id),
    FOREIGN KEY (ingredient_id) REFERENCES ingredient (id)
)