-- Created by eric on 4/16/26 - 17:38:24
CREATE TABLE recipe_ingredient
(
    recipe_id     UUID           NOT NULL,
    ingredient_id INT            NOT NULL,
    quantity      DECIMAL(10, 3) NOT NULL,
    unit          TEXT           NOT NULL,
    PRIMARY KEY (recipe_id, ingredient_id),
    FOREIGN KEY (recipe_id) REFERENCES recipe (id),
    FOREIGN KEY (ingredient_id) REFERENCES ingredient (id)
);