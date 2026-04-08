-- Created by eric on 4/7/26 - 15:27:33
CREATE TABLE provider
(
    id           UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name         TEXT NOT NULL,
    phone_number TEXT,
    email        TEXT,
    address      TEXT
)