-- Created by eric on 4/10/26 - 18:43:49
CREATE TABLE branch
(
    id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    address_line_1      TEXT NOT NULL,
    address_line_2      TEXT,
    zip_code            TEXT,
    locality            TEXT NOT NULL,
    administrative_area TEXT NOT NULL,
    country             TEXT NOT NULL
);

ALTER TABLE host_user
    ADD COLUMN branch_id UUID NOT NULL DEFAULT gen_random_uuid(),
    ADD FOREIGN KEY (branch_id) REFERENCES branch (id);

ALTER TABLE host_user ALTER COLUMN branch_id DROP DEFAULT;