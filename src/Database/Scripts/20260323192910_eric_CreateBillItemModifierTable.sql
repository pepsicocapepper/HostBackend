-- Created by eric on 3/23/26 - 14:47:05

CREATE TABLE bill_item_modifier
(
    bill_item_id UUID           NOT NULL,
    modifier_id  UUID           NOT NULL,
    price        DECIMAL(19, 4) NOT NULL,
    denomination denomination   NOT NULL,
    PRIMARY KEY (bill_item_id, modifier_id),
    FOREIGN KEY (bill_item_id) REFERENCES bill_item (id),
    FOREIGN KEY (modifier_id) REFERENCES modifier_element (id)
)