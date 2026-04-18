-- Created by eric on 4/14/26 - 14:16:13
CREATE TABLE branch_ingredient
(
    ingredient_id INT            NOT NULL,
    branch_id     UUID           NOT NULL,
    quantity      DECIMAL(10, 3) NOT NULL,
    unit          TEXT           NOT NULL,
    PRIMARY KEY (ingredient_id, branch_id),
    FOREIGN KEY (ingredient_id) REFERENCES ingredient (id),
    FOREIGN KEY (branch_id) REFERENCES branch (id)
)
