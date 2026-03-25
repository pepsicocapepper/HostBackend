-- Created by eric on 3/19/26 - 15:08:46

CREATE TABLE item_price
(
    id           SERIAL PRIMARY KEY,
    price        DECIMAL(19, 4) NOT NULL,
    denomination denomination   NOT NULL,
    item_id      INT            NOT NULL,
    FOREIGN KEY (item_id) REFERENCES item (id),
    UNIQUE (denomination, item_id)
)
