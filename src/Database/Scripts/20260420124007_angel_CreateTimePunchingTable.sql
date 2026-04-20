-- Created by angel on 04/20/2026 - 12:40
CREATE TABLE punching_times
(
    id         UUID PRIMARY KEY  DEFAULT gen_random_uuid(),
    in_or_out  CHAR         NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    FOREIGN KEY (user_id) REFERENCES host_user (id)
);

-- //tabla staffing
-- //campo staffing en usuario
-- //horas de usuarios por staffing o branch
-- //poder editar registros