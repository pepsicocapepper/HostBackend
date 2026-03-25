-- Created by eric on 3/22/26 - 19:23:06

CREATE TABLE modifier_group_element
(
    group_id   UUID NOT NULL,
    element_id UUID NOT NULL,
    PRIMARY KEY (group_id, element_id),
    FOREIGN KEY (group_id) REFERENCES modifier_group (id),
    FOREIGN KEY (element_id) REFERENCES modifier_element (id),
    UNIQUE (group_id, element_id)
)