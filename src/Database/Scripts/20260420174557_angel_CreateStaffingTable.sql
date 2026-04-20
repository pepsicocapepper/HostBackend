-- Created by angel on 04/20/2026 - 12:40
CREATE TABLE staffing
(
    id         UUID PRIMARY KEY  DEFAULT gen_random_uuid(),
    name        string         NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
);

-- //tabla staffing
-- //campo staffing en usuario
-- //horas de usuarios por staffing o branch
-- //poder editar registros