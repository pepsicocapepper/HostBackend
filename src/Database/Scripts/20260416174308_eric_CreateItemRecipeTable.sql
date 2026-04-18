-- Created by eric on 4/16/26 - 17:43:08
CREATE TABLE item_recipe
(
    item_id   INT  NOT NULL,
    recipe_id UUID NOT NULL,
    PRIMARY KEY (item_id, recipe_id),
    FOREIGN KEY (item_id) REFERENCES item (id),
    FOREIGN KEY (recipe_id) REFERENCES recipe (id)
);