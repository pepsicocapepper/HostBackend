-- Created by eric on 3/2/26 - 17:29:53
CREATE TABLE refresh_token
(
    id          SERIAL4 PRIMARY KEY,
    token       TEXT        NOT NULL,
    jwt_id      TEXT        NOT NULL,
    expiry_date TIMESTAMPTZ NOT NULL DEFAULT now(),
    invalidated BOOLEAN     NOT NULL DEFAULT FALSE,
    user_id     UUID        NOT NULL,
    FOREIGN KEY (user_id) REFERENCES host_user (id)
);
