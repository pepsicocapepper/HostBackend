-- Created by angel on 04/20/2026 - 12:40
CREATE TABLE user_punch_time
(
    id         SERIAL PRIMARY KEY,
    is_entrance  BOOLEAN    NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    user_id     UUID        NOT NULL,
    FOREIGN KEY (user_id) REFERENCES host_user (id)
);
