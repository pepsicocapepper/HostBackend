-- Created by eric on 2/23/26 - 00:46:31
-- Modified by angel on 03/17/2026 - 00:18
CREATE TABLE host_user
(
    id         UUID PRIMARY KEY     DEFAULT gen_random_uuid(),
    name       TEXT        NOT NULL,
    surname    TEXT        NOT NULL,
    job_title    TEXT        NOT NULL,
    phone   TEXT NULL,
    pin        TEXT        NOT NULL UNIQUE,
    active     BOOLEAN       NULL DEFAULT true,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);