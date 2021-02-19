import asyncio
import os
import pathlib
import sys


def check_returncode(returncode: int) -> None:
    if returncode != 0:
        sys.exit(-1)


async def find_msbuild_location() -> pathlib.Path:
    # location of vswhere (only for VS2017 and later)
    vswhere_path = pathlib.Path(os.environ["ProgramFiles(x86)"]).joinpath("Microsoft Visual Studio/Installer/vswhere.exe")
    vswhere_cmd = f"\"{vswhere_path}\" -latest -property \"installationPath\""
    vswhere_proc = await asyncio.create_subprocess_shell(vswhere_cmd, stdout=asyncio.subprocess.PIPE, stderr=asyncio.subprocess.DEVNULL)
    vswhere_stdout, _ = await vswhere_proc.communicate()
    check_returncode(vswhere_proc.returncode)
    vs_installation_path = pathlib.Path(vswhere_stdout.decode("utf-8").rstrip())
    if not vs_installation_path:
        raise ValueError("vs installation path")
    msbuild_apps = sorted(vs_installation_path.glob("**/MSBuild.exe"), key=lambda path: len(str(path)))
    if not msbuild_apps:
        raise ValueError("msbuild app instances")
    return msbuild_apps[0]


async def restore_nuget_packages(nuget_path: pathlib.Path, dest_path: pathlib.Path) -> None:
    nuget_cmd = f"\"{nuget_path}\" restore \"{dest_path}\""
    nuget_proc = await asyncio.create_subprocess_shell(nuget_cmd, stdout=asyncio.subprocess.DEVNULL, stderr=asyncio.subprocess.STDOUT)
    check_returncode(await nuget_proc.wait())


async def build_solution(msbuild_path: pathlib.Path, solution_path: pathlib.Path) -> None:
    msbuild_cmd = f"chcp 65001 & \"{msbuild_path}\" \"{solution_path}\""
    msbuild_proc = await asyncio.create_subprocess_shell(msbuild_cmd, stdout=asyncio.subprocess.PIPE, stderr=asyncio.subprocess.DEVNULL)
    msbuild_stdout, _ = await msbuild_proc.communicate()
    if msbuild_proc.returncode != 0:
        sys.stdout.buffer.write(msbuild_stdout)
    check_returncode(msbuild_proc.returncode)


async def main() -> None:
    base_path = pathlib.Path(__file__).resolve().parent
    nuget_path = base_path.joinpath("../../external/nuget-5.8.1/nuget.exe")
    solution_path = base_path.joinpath("../SourceCodeAnalysis.sln")
    # find msbuild utility
    msbuild_path = await find_msbuild_location()
    # restore nuget packages
    await restore_nuget_packages(nuget_path, solution_path)
    # build solution
    await build_solution(msbuild_path, solution_path)


asyncio.run(main())