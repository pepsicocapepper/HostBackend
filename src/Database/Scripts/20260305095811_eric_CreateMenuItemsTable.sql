-- Created by eric on 3/5/26 - 09:58:11
CREATE TABLE menu_item
(
    menu_id       INT NOT NULL,
    item_id       INT NOT NULL,
    display_order INT NOT NULL,
    UNIQUE (menu_id, item_id),
    FOREIGN KEY (menu_id) REFERENCES menu (id),
    FOREIGN KEY (item_id) REFERENCES item (id)
);