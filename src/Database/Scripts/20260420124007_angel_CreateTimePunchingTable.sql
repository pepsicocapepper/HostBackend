-- Created by angel on 04/20/2026 - 12:40
CREATE TYPE "punching_type" as enum(
'I',
'O'
);

CREATE TABLE punching_times
(
    id         UUID PRIMARY KEY  DEFAULT gen_random_uuid(),
    in_or_out  punching_type    NOT NULL,
    active      BOOLEAN     NOT NULL DEFAULT true(),
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    user_id     UUID        NOT NULL,
    FOREIGN KEY (user_id) REFERENCES host_user (id)
);
