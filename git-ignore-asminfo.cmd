@echo off
for /f %%f in ('git ls-files *AssemblyInfo.cs') do echo %%f && git update-index --verbose --skip-worktree %%f