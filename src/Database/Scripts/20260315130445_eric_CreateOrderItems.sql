-- Created by eric on 3/15/26 - 13:04:45
CREATE TABLE bill_item
(
    bill_id  UUID NOT NULL,
    item_id  INT  NOT NULL,
    quantity INT  NOT NULL,
    PRIMARY KEY (bill_id, item_id),
    FOREIGN KEY (bill_id) REFERENCES bill (id),
    FOREIGN KEY (item_id) REFERENCES item (id)
)