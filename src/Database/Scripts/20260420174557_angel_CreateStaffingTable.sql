-- Created by angel on 04/20/2026 - 12:40
CREATE TABLE staffing
(
    id         UUID PRIMARY KEY  DEFAULT gen_random_uuid(),
    name       TEXT         NOT NULL,
    active      BOOLEAN NOT NULL DEFAULT true,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

ALTER TABLE host_user
    ADD COLUMN staffing_id UUID NOT NULL,
    ADD FOREIGN KEY (staffing_id) REFERENCES staffing (id);
-- mueve todo en user para que jale el endpoint, luego el frontend tambien