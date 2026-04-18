-- Created by eric on 4/16/26 - 17:34:41
CREATE TABLE recipe
(
    id    UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name  TEXT   NOT NULL UNIQUE,
    steps TEXT[] NOT NULL  DEFAULT array []::text[]
);