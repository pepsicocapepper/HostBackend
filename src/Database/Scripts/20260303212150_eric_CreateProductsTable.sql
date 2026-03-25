-- Created by eric on 3/3/26 - 21:21:50
CREATE TYPE pricing_model AS ENUM ('base', 'size');
CREATE TABLE item
(
    id            SERIAL4 PRIMARY KEY,
    name          TEXT          NOT NULL UNIQUE,
    pos_name      TEXT UNIQUE,
    color         BYTEA,
    pricing_model pricing_model NOT NULL DEFAULT 'base',
    created_by    UUID          NOT NULL,
    created_at    TIMESTAMPTZ   NOT NULL DEFAULT now(),
    updated_by    UUID,
    updated_at    TIMESTAMPTZ,
    FOREIGN KEY (created_by) REFERENCES host_user (id),
    FOREIGN KEY (updated_by) REFERENCES host_user (id)
)