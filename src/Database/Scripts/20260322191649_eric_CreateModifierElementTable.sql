-- Created by eric on 3/22/26 - 19:16:49

CREATE TABLE modifier_element
(
    id           UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name         TEXT           NOT NULL,
    price        DECIMAL(19, 4) NOT NULL,
    denomination denomination   NOT NULL
)