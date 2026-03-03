#!/bin/bash

GREEN='\033[0;32m'
NC='\033[0m'

echo -e "${GREEN}=== Atualizando Banco de Dados ===${NC}"

dotnet ef database update \
    --project src/Domus.Infrastructure \
    --startup-project src/Domus.Api

echo -e "${GREEN}✅ Processo finalizado.${NC}"