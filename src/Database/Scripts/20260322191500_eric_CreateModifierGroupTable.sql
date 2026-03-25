-- Created by eric on 3/22/26 - 19:15:00

CREATE TABLE modifier_group
(
    id   UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name TEXT NOT NULL UNIQUE
)