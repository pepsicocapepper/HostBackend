-- Created by eric on 2/23/26 - 00:46:31
CREATE TABLE host_user
(
    id         UUID PRIMARY KEY     DEFAULT gen_random_uuid(),
    name       TEXT        NOT NULL,
    surname    TEXT        NOT NULL,
    pin        TEXT        NOT NULL UNIQUE,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);