-- Created by eric on 3/23/26 - 19:27:14
CREATE TABLE bill_item
(
    id           UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    bill_id      UUID           NOT NULL,
    item_id      INT            NOT NULL,
    price        DECIMAL(19, 4) NOT NULL,
    denomination denomination   NOT NULL,
    quantity     INT            NOT NULL,
    FOREIGN KEY (bill_id) REFERENCES bill (id),
    FOREIGN KEY (item_id) REFERENCES item (id)
)
