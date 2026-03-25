-- Created by eric on 3/22/26 - 19:29:47

CREATE TABLE item_modifier_group
(
    id            SERIAL PRIMARY KEY,
    item_id       INT  NOT NULL,
    group_id      UUID NOT NULL,
    display_order INT  NOT NULL,
    FOREIGN KEY (group_id) REFERENCES modifier_group (id),
    FOREIGN KEY (item_id) REFERENCES item (id),
    UNIQUE (item_id, group_id)
)